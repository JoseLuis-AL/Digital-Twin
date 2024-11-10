import sqlite3
import paho.mqtt.client as mqtt
from datetime import datetime
from threading import Thread
from queue import Queue

# Configuración de la base de datos SQLite.
db_path = 'database/DigitalTwin.db'
conn = sqlite3.connect(db_path, check_same_thread=False)
cursor = conn.cursor()
cursor.execute('PRAGMA journal_mode=WAL;')

# Consulta para insertar datos.
insert_query = '''
    INSERT INTO MotorDT (Time, Counts, State, Motor)
    VALUES (?, ?, ?, ?)
'''

# TODO: Remover estados.
state = "Normal"
motor = "ON"

# Cola para almacenar los datos.
data_queue = Queue()

# Función para escribir en la base de datos.
def db_writer():
    while True:
        current_time, count = data_queue.get()
        try:
            cursor.execute(insert_query, (current_time, count, state, motor))
            conn.commit()
        except Exception as e:
            print(f"Error al escribir en la base de datos: {e}")
        data_queue.task_done()

# Iniciar el hilo de escritura.
writer_thread = Thread(target=db_writer, daemon=True)
writer_thread.start()

def on_message(client, userdata, msg):
    try:
        count = int(msg.payload.decode())
        current_time = datetime.now().strftime('%Y-%m-%d %H:%M:%S')
        data_queue.put((current_time, count))
    except Exception as e:
        print(f"Error al procesar el mensaje: {e}")

client = mqtt.Client()
client.on_message = on_message
client.connect("148.225.99.80", 1883, 60)
client.subscribe("test_count")
client.loop_start()

try:
    print('Iniciando recolección de counts.')
    while True:
        pass
except KeyboardInterrupt:
    client.loop_stop()
    conn.close()
