from flask import Flask, jsonify, request, make_response
import sqlite3
import hashlib

app = Flask(__name__)

@app.route('/auth', methods=['POST'])
def authenticate_user():
    # Obtener los datos JSON del cuerpo de la solicitud
    data = request.get_json()

    # Comprobar que la petición contiene el usuario y la contraseña.
    if not data or 'usuario' not in data or 'password' not in data:
        return make_response(jsonify({'message': 'Parámetros faltantes'}), 400)

    usuario = data['usuario']
    password = data['password']

    # Encriptar con SHA-256.
    password_hash = hashlib.sha256(password.encode('utf-8')).hexdigest()

    # Conectar a la base de datos SQLite.
    conn = sqlite3.connect('/home/pi/Digital-Twin/DataBase/sensor_data.db')
    cursor = conn.cursor()

    # Buscar el usuario en la base de datos.
    cursor.execute("SELECT password_hash FROM users WHERE username_hash=?", (usuario,))
    result = cursor.fetchone()

    if not result:
        # Usuario no encontrado.
        conn.close()
        return make_response(jsonify({'message': 'Usuario no encontrado'}), 404)

    stored_password_hash = result[0]

    # La password es incorrecta.
    if password_hash != stored_password_hash:
        conn.close()
        return make_response(jsonify({'message': 'Password incorrecta'}), 401)

    # Autenticación exitosa.
    conn.close()
    return make_response(jsonify({'message': 'Usuario correcto'}), 200)

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)

