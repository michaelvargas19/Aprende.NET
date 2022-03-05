
# Ejercicio de Introducción – Calculadora interoperable

La empresa de contabilidad Sumando Y Restando ha solicitado al equipo de desarrollo Aprende.Net, implementar una herramienta digital para el cálculo de operaciones, básicas bajo la plataforma .Net.

Las historias de usuario son:
-	Como usuario del sistema quiero tener un portal web para el cálculo de operaciones básicas
-	Yo como usuario del sistema quiero consultar el histórico de operaciones ordenado por fecha de operación donde el primero sea el más reciente
-	Como arquitecto de soluciones requiero tener interoperabilidad en el sistema para poder integrar más componentes que requieran estas operaciones
-	Como desarrollador quiero que se centralice la lógica del negocio para evitar la duplicación de código y tener mantenibilidad en el sistema

Para la implementación de la solución se han definido las siguientes Épicas aprobadas por la empresa Sumando Y Restando:

## Épica 1 – Calculo de operación
1.	Clonar la rama ejercicio/introducción
2.	Agregue una clase llamada *CalculadoraModel* en la carpeta *Models* del proyecto *Introduccion.NET.Ejercicio.Introduccion.MVC,* con la siguiente estructura:
-	PrimerOperador
-	SegundoOperador
-	Operación
-	Resultado
4.	Generar un controlador *MVC* en la carpeta *Controllers* con operaciones de lectura y escritura llamado *CalculadoraController*
5.	Genere una vista tipo Razor para las acciones Create y View en la carpeta *Views*, manteniendo la estructura MVC. Se sugiere usar el wizard para generar la vista con las y modelos definidos en pasos anteriores

*Nota: Luego de realizar este paso podrá ejecutar la aplicación y consultar el formulario con la uri /Calculadora/Create*

5.	En el método POST llamado Create del controlador *CalculadoraController* agregue la lógica de negocio que corresponda para la calculadora.
a.	Redireccione esta respuesta a la vista *Calculadora/View* enviando los datos de la calculadora para mostrarlos en pantalla
6.	Agregar un botón o enlace en la vista *Home* y *Calculadora/View* para acceder a la calculadora y realizar más operaciones

**Para continuar con el desarrollo a partir de este punto es necesario tener claro el concepto de inyección de dependencias.**

## Epica 2 – Historial de operaciones
7.	Agregue un nuevo componente o capa al proyecto llamado *Introduccion.NET.Ejercicio.Introduccion.Repository* para persistir el historial de operaciones en una base de datos. 
a.	Puede usar base de datos que prefiera siempre y cuando se almacene la siguiente información:
-	Identificador único
-	PrimerOperador
-	SegundoOperador
-	Operación
-	Resultado
-	FechaCalculo
-	Usuario
8.	Agregue una vista para consultar el historial de operaciones almacenado en la base de datos ordenado por la fecha del cálculo en forma descendente 

*Nota: El usuario se debe capturar en el momento de que diligencia la operación.*

## Epica 3
9.	Para satisfacer el requisito de Interoperatividad se ha decidido exponer servicios web que realicen las mismas operaciones que la calculadora hecha en el proyecto *MVC*. Para esto se ha creado el proyecto *Introduccion.NET.Ejercicio.Introduccion.API* en la solución quien expondrá los siguientes servicios web:

- GET: Consultar historial de operaciones
-	POST: Hacer operación

Complete la lógica de negocio en cada una de la definición de los servicios, haciendo uso del componente de persistencia.

## Epica 4
10.	Se ha detectado que la lógica del negocio (cálculo de operaciones) se está repitiendo en dos componentes *Introduccion.NET.Ejercicio.Introduccion.MVC* y *Introduccion.NET.Ejercicio.Introduccion.API*, por esto se ha decidido centralizar esta lógica en un proyecto llamado *Introduccion.NET.Ejercicio.Introduccion.Negocio*.



