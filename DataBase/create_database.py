# create_database.py
import sqlite3

def create_database():
    conn = sqlite3.connect('sensor_data.db')
    c = conn.cursor()
    
    # Crear tabla
    c.execute('''CREATE TABLE IF NOT EXISTS sensor_readings
                 (device_id TEXT,
                  time TIMESTAMP, 
                  counts INTEGER, 
                  state_connection TEXT, 
                  state_motor TEXT,
                  PRIMARY KEY (device_id, time))''')
    
    # Crear un índice en time para mejorar el rendimiento de las consultas
    c.execute('''CREATE INDEX IF NOT EXISTS idx_time ON sensor_readings(time)''')
    
    conn.commit()
    conn.close()
    print("Base de datos creada exitosamente")

if __name__ == "__main__":
    create_database()
