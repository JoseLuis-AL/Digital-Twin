#!/usr/bin/env python3
import paho.mqtt.client as mqtt
import json
import sqlite3
from datetime import datetime
import logging
import signal
import sys

# Configurar logging
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(levelname)s - %(message)s',
    handlers=[
        logging.FileHandler('/home/pi/Digital-Twin/DataBase/mqtt_service.log'),
        logging.StreamHandler()
    ]
)

# Configuración MQTT
MQTT_BROKER = "localhost"
MQTT_PORT = 1883
MQTT_TOPIC = "sensor/data"
MQTT_USER = "digitalTwin"
MQTT_PASSWORD = "VMTQHGLEOISJHULSOH"

# Configuración Base de datos
DB_PATH = '/home/pi/Digital-Twin/DataBase/sensor_data.db'

def create_database():
    conn = sqlite3.connect(DB_PATH)
    # Configurar WAL
    conn.execute('PRAGMA journal_mode=WAL')
    conn.execute('PRAGMA synchronous=NORMAL')
    conn.execute('PRAGMA busy_timeout=5000')
    
    c = conn.cursor()
    c.execute('''CREATE TABLE IF NOT EXISTS sensor_readings
                 (id INTEGER PRIMARY KEY AUTOINCREMENT,
                  device_id TEXT,
                  time TIMESTAMP,
                  counts INTEGER,
                  state_connection TEXT,
                  state_motor TEXT)''')
    conn.commit()
    conn.close()

def insert_data(device_id, counts, state_connection, state_motor):
    try:
        conn = sqlite3.connect(DB_PATH, timeout=20)  # Aumentar timeout
        # Configurar WAL para cada conexión
        conn.execute('PRAGMA journal_mode=WAL')
        conn.execute('PRAGMA synchronous=NORMAL')
        conn.execute('PRAGMA busy_timeout=5000')
        
        c = conn.cursor()
        current_time = datetime.now().strftime('%Y-%m-%d %H:%M:%S.%f')
        c.execute('''INSERT INTO sensor_readings 
                     (device_id, time, counts, state_connection, state_motor)
                     VALUES (?, ?, ?, ?, ?)''',
                     (device_id, current_time, counts, state_connection, state_motor))
        conn.commit()
        logging.info(f"Datos insertados: Device={device_id}, Time={current_time}, Counts={counts}")
    except sqlite3.Error as e:
        logging.error(f"Error al insertar datos: {e}")
    finally:
        conn.close()

def on_connect(client, userdata, flags, rc):
    if rc == 0:
        logging.info("Conectado exitosamente al broker MQTT")
        client.subscribe(MQTT_TOPIC)
        logging.info(f"Suscrito al topic: {MQTT_TOPIC}")
    else:
        logging.error(f"Fallo en la conexión. Código de retorno: {rc}")

def on_message(client, userdata, msg):
    try:
        payload = json.loads(msg.payload.decode())
        device_id = payload.get('device_id')
        counts = payload.get('counts')
        state_connection = payload.get('state_connection')
        state_motor = payload.get('state_motor')
        
        if all(v is not None for v in [device_id, counts, state_connection, state_motor]):
            insert_data(device_id, counts, state_connection, state_motor)
        else:
            logging.warning("Datos incompletos recibidos")
    except json.JSONDecodeError as e:
        logging.error(f"Error decodificando JSON: {e}")
    except Exception as e:
        logging.error(f"Error procesando mensaje: {e}")

def signal_handler(signum, frame):
    logging.info("Señal de terminación recibida. Cerrando aplicación...")
    client.disconnect()
    sys.exit(0)

if __name__ == "__main__":
    # Registrar manejador de señales
    signal.signal(signal.SIGTERM, signal_handler)
    signal.signal(signal.SIGINT, signal_handler)

    # Crear base de datos si no existe
    create_database()

    # Configurar cliente MQTT
    client = mqtt.Client()
    client.username_pw_set(MQTT_USER, MQTT_PASSWORD)  # Agregar autenticación
    client.on_connect = on_connect
    client.on_message = on_message

    try:
        client.connect(MQTT_BROKER, MQTT_PORT, 60)
        logging.info("Iniciando loop MQTT...")
        client.loop_forever()
    except Exception as e:
        logging.error(f"Error en la aplicación: {e}")
