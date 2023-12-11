## Digital Twin
Equipo:

- Aguilera Luzania José Luis.
- Corral Valdez Jesús Giovanni.
- Varela Luna Diego Josue.
- Salido Varela Sebastian.

Trabajo realizado para la materia Sistemas Ciberfísicos de la Maestría en Ingeniería en Internet de las Cosas e Inteligencia Artificial de la Universidad de Sonora.

### Introducción  
<p align="justify">
En la actualidad, estamos inmersos en la 3ra revolución industrial, la era de la automatización, en la que el procesamiento de la información ha alcanzado un nivel tan sofisticado que se lleva a cabo por medio de procesadores con computadoras operadas por robots en diversos sectores de la industria manufacturera. Esta combinación de tecnología, que permite las máquinas dejar de ejecutar solo tareas físicas y comenzar a tomar decisiones basadas en grandes volúmenes de datos y algoritmos de inteligencia artificial que tienen repercusiones en sus operaciones, ha transformado la forma en la que se lleva a cabo la producción y está impulsando la eficiencia y precisión de los procesos a nuevos niveles. Esta conexión entre el mundo físico y el mundo digital, donde la automatización se basa en el internet, la inteligencia artificial y la representación digital de los sistemas físicos es solo el comienzo de la 4ta revolución industrial. Las fábricas modernas se convertirán en un conjunto de herramientas de inteligencia artificial, internet de las cosas y sistemas ciberfísicos.
</p>
<p align="justify">
El pilar fundamental de la 4ta revolución industrial son los Sistemas Ciberfísicos o Cyber-Physics System (CPS). Los sistemas ciberfísico son sistemas de colaboración computacional que cuentan con una conexión intensa con el mundo físico que los rodea y al mismo tiempo tienen acceso a grandes volúmenes de datos y proveen un margen de servicios disponibles en internet, es la unión entre el mundo físico y el mundo digital, no es suficiente comprender el comportamiento de cada uno de los componentes físicos y los componentes computacionales, se debe entender la relación entre ellos. En otras palabras, los sistemas ciberfísico pueden caracterizarse generalmente como “Sistemas físicos y de ingeniería cuyas operaciones son monitoreadas, controladas, coordinadas e integradas por un núcleo informático y de comunicación” [1].
</p>

<p align="center">
  <img src="https://github.com/JoseLuis-AL/Digital-Twin/blob/main/Images/Sistema%20Ciberf%C3%ADsico.png?raw=true" alt="Sublime's custom image" width="650"/>
</p>

**Gemelo digital**  
 Un gemelo digital o Digital Twin es una réplica digital del sistema conectada y sincronizada en tiempo real con su contraparte física.

### Descripción del sistema  
<p align="justify">
El sistema utilizado en la elaboración del proyecto, descrito de manera simple, es un motor DC con encoder que convierte su movimiento rotacional en un desplazamiento lineal, específicamente moviendo un perfil de hecho de aluminio. El sistema se encuentra equipado con componentes electrónicos, como un Arduino utilizado para controlar la fuerza y velocidad del movimiento; un módulo ESP8266, que gestionan la lectura de los datos del encoder y los transmite por medio de una conexión a internet usando el protocolo MQTT, los datos son recibidos por un flujo de trabajo en Node-Red para su análisis y por una aplicación de Unity para la representación gráfica en tiempo real como se muestra en la siguiente figura.
</p>

<p align="center">
  <img src="https://github.com/JoseLuis-AL/Digital-Twin/blob/main/Images/descripcion_sistema.png?raw=true" alt="Sublime's custom image" width="550"/>
</p>

### Resultado
<p align="justify">
Se creó una réplica digital del sistema que se encuentra conectada y sincronizada en tiempo real con su contraparte física. La réplica es capaz de mostrar los cambios en tiempo real del sistema físico.
</p>

<p align="center">
  <img src="https://github.com/JoseLuis-AL/Digital-Twin/blob/main/Images/App_Final.png?raw=true" alt="Sublime's custom image"/>
</p>

### Referencias  
[1].R. Rajkumar, I. Lee, L. Sha and J. Stankovic, "Cyber-physical systems: The next computing revolution," Design Automation Conference, Anaheim, CA, USA, 2010, pp. 731-736, doi: 10.1145/1837274.1837461.  
[2].“Control Tutorials for MATLAB and Simulink - Time-response Analysis of a DC motor.” https://ctms.engin.umich.edu/CTMS/index.php?aux=Activities_DCmotorA  
[3].“Simulación y diseño basado en modelos con Simulink.” https://la.mathworks.com/products/simulink.html  
[4].K. Ogata, Ingeniería de control moderna 5 ed. PRENTICE HALL, 2010.  
[5].Meganburroughs, “Encoder Resolution: ppr and cpr,” Precision Microdrives, Jun. 2022, [Online]. Available: https://www.precisionmicrodrives.com/encoder-resolution-ppr-and-cpr  
[6].Libretexts, «3.3: Controladores PI, PD y PID», LibreTexts Español, 2 de noviembre de 2022.  
 https://espanol.libretexts.org/Ingenieria/Ingenier%C3%ADa_Industrial_y_de_Sistemas/Libro%3A_Introducci%C3%B3n_a_los_Sistemas_de_Control_(Iqbal)/03%3A_Modelos_de_sistemas_de_control_de_retroalimentaci%C3%B3n/3.3%3A_Controladores_PI%2C_PD_y_PID
[7].Control tutorials for MATLAB and Simulink - PI Control of DC Motor speed. https://ctms.engin.umich.edu/CTMS/index.php?aux=Activities_DCmotorB  
[8].E. A. Lee y S. A. Seshia, Introduction to Embedded Systems - a Cyber-Physical Systems approach. 2013. [En línea]. Disponible en: https://ci.nii.ac.jp/ncid/BB04045657  
[9].Eclipse Foundation. (2023). Mosquitto - An Open Source MQTT Broker. Disponible en: https://mosquitto.org/  
[11].C. Valdivia, Sistemas de control continuos y discretos, Ediciones Paraninfo, 2012.  
[10].M2MQTT. (2023). M2MQTT - MQTT Client Library for .Net and WinRT. Disponible en: https://m2mqtt.wordpress.com/  
