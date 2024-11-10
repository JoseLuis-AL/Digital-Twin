# create_database.py
import sqlite3

def create_database():
    conn = sqlite3.connect('sensor_data.db')
    c = conn.cursor()

    # Crear tabla para los datos del motor
    c.execute('''CREATE TABLE IF NOT EXISTS sensor_readings
                 (device_id TEXT,
                  time TIMESTAMP,
                  counts INTEGER,
                  motor_direction TEXT,
                  motor_state TEXT,
                  PRIMARY KEY (device_id, time))''')

    # Crear un índice en time para mejorar el rendimiento de las consultas.
    c.execute('''CREATE INDEX IF NOT EXISTS idx_time ON sensor_readings(time)''')
    
    # Crear la tabla para los usuarios.
    c.execute('''CREATE TABLE IF NOT EXISTS users (
                 id INTEGER PRIMARY KEY AUTOINCREMENT,
                 username TEXT NOT NULL,
                 password TEXT NOT NULL
                 );''')
    
    # Crear el usuario admin.
    c.execute('''INSERT INTO users (username, password)
                 SELECT 'admin', 'd763a651e8bd935b382a15e699676c20'
                 WHERE NOT EXISTS (
                     SELECT 1 FROM users WHERE username = 'admin'
                 );''')

    # Terminar las modificaciones y cerrar la base de datos.
    conn.commit()
    conn.close()
    print("Base de datos creada exitosamente")

if __name__ == "__main__":
    create_database()