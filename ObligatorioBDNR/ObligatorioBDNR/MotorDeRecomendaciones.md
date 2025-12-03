# Escenario: Duolingo

Duolingo es una plataforma líder en el aprendizaje de idiomas que ha transformado la forma en
que millones de personas estudian. Con un enfoque gamificado, la plataforma ofrece cursos en
más de 40 idiomas y ha expandido recientemente su oferta hacia otras áreas como
matemáticas y música. Su modelo freemium, accesible desde dispositivos móviles y web
genera una enorme cantidad de datos provenientes del comportamiento de los usuarios,
interacciones con el contenido, ejercicios resueltos y mucho más.  
La arquitectura de la plataforma se basa en microservicios, donde cada subsistema maneja su
propia base de datos, permitiendo elegir la tecnología que mejor se ajuste a los requerimientos
funcionales y no funcionales.

A continuación se presentan los subsistemas principales sobre los que trabajará el obligatorio:
- Gestión de usuarios y perfiles
- Sistema de lecciones y ejercicios
- Motor de recomendaciones
- Foros y comunidad
- Analítica y seguimiento de actividad
- Sistema de notificaciones
- Tienda virtual y suscripciones
- Privacidad y seguridad  

Estos subsistemas son independientes unos de los otros por lo que la comunicación entre ellos
se realiza mediante servicios y cada subsistema almacena los datos en la tecnología y base de
datos que mejor se adapte a sus necesidades. A continuación se describe cada subsistema.


## Motor de recomendaciones

Duolingo recomienda contenido educativo a los usuarios basándose en diversos criterios para
optimizar su aprendizaje. Este sistema de recomendaciones no solo ayuda a personalizar la
experiencia de estudio, sino que también permite detectar áreas de mejora y reforzar
conocimientos clave a través de ejercicios específicos. 

El motor de sugerencias de Duolingo analiza el perfil del usuario, su progreso en los cursos, los
errores más frecuentes que ha cometido y sus preferencias de aprendizaje. A partir de estos
datos, se pueden generar ejercicios personalizados, reforzar habilidades que requieren mayor
práctica y sugerir nuevas unidades o desafíos adicionales. 

Para lograr esto, Duolingo utiliza distintas técnicas avanzadas que permiten detectar similitudes
entre los patrones de aprendizaje de distintos usuarios. Si un estudiante tiene dificultades con
una estructura gramatical específica, el sistema puede recomendar ejercicios que han sido
útiles para otros con problemas similares. De la misma manera, si un usuario ha mostrado
interés en cierto tipo de contenido, se le pueden sugerir actividades relacionadas para
mantener su motivación.  

Este enfoque adaptativo permite mejorar la experiencia del usuario y aumentar la eficiencia del
aprendizaje. Además, gracias al análisis de datos a gran escala, es posible optimizar
continuamente el sistema, asegurando que las recomendaciones sean cada vez más precisas y
efectivas.  


## Para responder a al Motor de Recomendaciones se modeló lo siguiente:

### Motor de recomendaciones
#### Motor de base de datos
Se usará una base de datos orientada a grafos como Neo4j ya que estas son ideales para usar en 
sistemas de recomendaciones en tiempo real, donde se pueden analizar datos correlacionados y donde
el crecimiento de nodos es constante.  
Neo4j se especializa en operar sobre relaciones, por lo que tiene sentido utilizarlo en este 
subsistema ya que debe detectar similitudes entre los patrones de aprendizaje de distintos usuarios, 
considerando su progreso, los errores o dificultades que pueda tener, así como similitudes entre usuarios.  

#### Requerimientos no funcionales
A continuación, enunciamos los requerimientos no funcionales que identificamos y en los que basamos la 
selección de usar Neo4j:

**Recomendaciones en tiempo real**
Las recomendaciones que se realicen deben hacerse en el momento en el que el usuario esté usando la 
aplicación. De esta forma, la base de datos a usar no puede demorar mucho tiempo en generar las 
recomendaciones.

**Flexibilidad**
El motor de recomendaciones debe ser capaz de incorporar nuevos tipos de ejercicios, nuevas 
recomendaciones, etc. al tratarse de una aplicación que evoluciona constantemente. Particularmente, 
Neo4j tiene un modelo con alta flexibilidad llamado Relationships-first approach que es ideal para 
manejar esta necesidad, como se puede ver en su página web o en su documento sobre el uso de Neo4j en 
recomendaciones, en el Anexo.

**Buen rendimiento con grandes cantidades de datos altamente relacionados**
Las entidades como los usuarios, los ejercicios, el progreso, etc. se tienen como nodos y forman un grafo
denso. Neo4j evita realizar JOINs como sí sucede en SQL y permite recorridos eficientes por relaciones
múltiples.

#### Modelo de datos previsto
En el modelo de datos para el subsistema de Motor de recomendaciones se tendrán nodos para las entidades
y para crear los grafos, habrán relaciones entre los nodos.

El subsistema tendrá los siguientes nodos:
- **Usuario:**
  - Es quien usa la aplicación y a quien se le mostrarán recomendaciones.
  - Tiene las siguientes propiedades:
    - idUsuario
    - username

- **Idioma:**
  - Tiene las siguientes propiedades:
    - idIdioma
    - nombre

- **Unidad:**
  - Bloque de contenido que tiene como propiedades:
    - idUnidad
    - nombre
    - posición
    - descripción
    - nivel

- **Habilidad:**
  - Competencia concreta de un idioma. Tiene como propiedades:
    - idHabilidad
    - nombre
    - descripción
    - categoría

- **Ejercicio:**
  - Actividad que debe resolver el usuario para avanzar en la unidad y tiene como propiedades:
    - idEjercicio
    - nombre
    - descripción
    - categoría

- **Error:**
  - Clasificación del tipo de error. Tiene las propiedades:
    - idError
    - descripción
    - categoría  

Por su parte se tienen las relaciones entre cada nodo:
- **Usuario-ESTUDIA-Idioma**  
    Un usuario puede estudiar uno o varios idiomas. Tiene las propiedades:
    - nivel
    - fechaInicio
    - fechaUltimaActividad

- **Unidad-DEL_IDIOMA-Idioma**

- **Habilidad-DEL_IDIOMA-Idioma**

- **Ejercicio-PERTENECE_A-Unidad**  
    Un ejercicio pertenece a una unidad y tiene como propiedad:
    - Posición

- **Ejercicio-REFUERZA-Habilidad**  
    Indica la habilidad que refuerza cierto ejercicio.

- **Usuario-REALIZA-Ejercicio**
    Indica cada intento de un usuario sobre un ejercicio. Tiene como propiedad:
    - fechaRealizado
    - resultado
    - tiempo
    - intentos

- **Usuario-FALLA_EN-Habilidad**
    Muestra parte del desempeño del usuario sobre una habilidad. Cuenta con la propiedad:
    - vecesFalladas

- **Usuario-SIMILAR_A-Usuario**
    De esta forma se pueden detectar patrones similares entre los usuarios.

- **Unidad-PRECEDE_A-Unidad**
    Las unidades de un mismo idioma tienen un orden de dictado.


#### Principales patrones de acceso identificados

En el motor de recomendaciones se identifican los siguientes patrones de acceso, en base a los tipos de 
análisis que Neo4j tiene como característicos para recomendaciones:

1. **Recomendación basada en las dificultades del usuario**  
   El subsistema de motor de recomendaciones debe obtener las habilidades en las que el usuario presenta mayores dificultades y luego buscar ejercicios que permitan reforzarlas
   Este patrón recorre el grafo de la siguiente forma:
   - Obtener las habilidades donde el usuario presenta fallas:
     - (:Usuario)-[:FALLA_EN]->(:Habilidad)  

   - Obtener los ejercicios que refuercen estas habilidades:
     - (:Ejercicio)-[:REFUERZA]->(:Habilidad)  

   - Filtrar los ejercicios según el idioma que el usuario está estudiando:
     - (:Usuario)-[:ESTUDIA]->(:Idioma)
     - (:Unidad)-[:DEL_IDIOMA]->(:Idioma)
     - (:Ejercicio)-[:PERTENECE_A]->(:Unidad)  
   La idea es poder recorrer el grafo para identificar datos importantes basándose en los errores y dificultades 
   del usuario, lo más rápido posible  
  

2. **Filtro basado en la similitud de usuarios**  
   El subsistema también tiene que considerar los comportamientos similares entre los usuarios con el fin de recomendar ejercicios que hayan sido útiles para estudiantes con perfiles similares. El recorrido del grafo sigue este sería:
   - Identificar usuarios similares con:
     - (:Usuario)-[:SIMILAR_A]->(:Usuario)

   - Obtener ejercicios que estos usuarios realizaron:
     - (:Usuario)-[:REALIZA]->(:Ejercicio)

   - Filtrar esos ejercicios por idioma usando las relaciones ESTUDIA y DEL_IDIOMA.  


3. **Recomendación basada en el contenido y la estructura del curso de cierto idioma**  
   Las recomendaciones también pueden basarse en el curso, reforzando contenidos ya dados o anticipar 
   los contenidos que se darán en el futuro. El recorrido podría ser:  
   - Obtener ejercicios pertenecientes a una unidad:  
     - (:Ejercicio)-[:PERTENECE_A]->(:Unidad)  

   - Obtener la estructura de avance entre unidades:  
     - (:Unidad)-[:PRECEDE_A]->(:Unidad)  

   - Detectar las habilidades asociadas a los ejercicios mediante:  
     - (:Ejercicio)-[:REFUERZA]->(:Habilidad)  

   - Filtrar todo el contenido por idioma con:
     - (:Unidad)-[:DEL_IDIOMA]->(:Idioma)
     - (:Habilidad)-[:DEL_IDIOMA]->(:Idioma)  

    La idea es poder recomendar ejercicios adecuados al nivel del usuario de cierto idioma.


4. **Recomendación según el idioma estudiado por el usuario**  
   Como los usuarios pueden estudiar más de un idioma a la vez, los patrones de acceso tienen que 
   realizarse dentro del subgrafo correspondiente al idioma particular que estudia. Se toma en cuenta:  
   - Idioma que estudia el usuario:
     - (:Usuario)-[:ESTUDIA]->(:Idioma)

   - Unidades y habilidades asociadas a ese idioma:
     - (:Unidad)-[:DEL_IDIOMA]->(:Idioma)
     - (:Habilidad)-[:DEL_IDIOMA]->(:Idioma)

   - Ejercicios pertenecientes a esas unidades:
     - (:Ejercicio)-[:PERTENECE_A]->(:Unidad)

5. **Consultas según diversos criterios**  
   El motor puede tener que combinar los criterios mencionados anteriormente para hacer recomendaciones 
   más precisas.


#### Modelado genérico del subsistema Motor de recomendaciones
Los 6 nodos y las 9 relaciones mencionadas anteriormente permiten modelar el subsistema de Motor de
recomendaciones de Duolingo.

#### Ejemplo
```
CREATE (u:Usuario {
idUsuario: ‘u1’,
username: ‘Laura’
});

CREATE (i:Idioma {
idIdioma: 'en',
nombre: 'Inglés'
});

MATCH (u:Usuario {idUsuario: 'u1'}), (i:Idioma {idIdioma: 'en'})
CREATE (u)-[:ESTUDIA {
nivel: 'A1',
fechaInicio: date('2025-01-10'),
fechaUltimaActividad: date('2025-09-25')
}]->(i);

CREATE (un:Unidad {
idUnidad: 'u_basic1_en',
nombre: 'Básico 1',
posicion: 1,
descripcion: 'Unidad introductoria de inglés',
nivel: 'A1'
});

MATCH (un:Unidad {idUnidad: 'u_basic1_en'}), (i:Idioma {idIdioma: 'en'})
CREATE (un)-[:DEL_IDIOMA]->(i);

CREATE (h:Habilidad {
idHabilidad: 'h_present_simple',
nombre: 'Present simple',
descripcion: 'Uso del present simple en una oración',
categoria: 'gramática'
});

MATCH (h:Habilidad {idHabilidad: 'h_present_simple'}), (i:Idioma {idIdioma: 'en'})
CREATE (h)-[:DEL_IDIOMA]->(i);

CREATE (e1:Ejercicio {
idEjercicio: 'e101',
nombre: 'Crear oraciones present simple 1',
descripcion: 'Crear oraciones negativas en present simple',
categoria: 'crear'
});

CREATE (e2:Ejercicio {
idEjercicio: 'e102',
nombre: 'Completar present simple 1',
descripcion: 'Completar los verbos en present simple',
categoria: 'completar'
});

MATCH (un:Unidad {idUnidad: 'u_basic1_en'})
MATCH (e1:Ejercicio {idEjercicio: 'e101'})
MATCH (e2:Ejercicio {idEjercicio: 'e102'})
CREATE (e1)-[:PERTENECE_A {posicion: 1}]->(un);
CREATE (e2)-[:PERTENECE_A {posicion: 2}]->(un);

MATCH (e1:Ejercicio {idEjercicio: 'e101'}), (h:Habilidad {idHabilidad: 'h_present_simple'})
CREATE (e1)-[:REFUERZA]->(h);

MATCH (u:Usuario {idUsuario: 'u1'}), (e1:Ejercicio {idEjercicio: 'e101'}), (h:Habilidad {idHabilidad: 'h_present_simple'})
CREATE (u)-[:REALIZA {
fechaRealizado: datetime('2025-09-25T10:30:00'),
resultado: 'incorrecto',
tiempo: 40,
intentos: 1
}]->(e1);

CREATE (u)-[:FALLA_EN {
vecesFalladas: 5
}]->(h);
``` 


#### Para consultar
Para consultar por los ejercicios para reforzar las habilidades donde un usuario tiene dificultades,
se haría la consulta en Cypher:  
```
MATCH (u:Usuario {idUsuario: 'u1'})-[:ESTUDIA]->(idI:Idioma)  
MATCH (u)-[f:FALLA_EN]->(h:Habilidad)  
MATCH (h)-[:DEL_IDIOMA]->(idI)  
MATCH (e:Ejercicio)-[:REFUERZA]->(h)  
MATCH (e)-[:PERTENECE_A]->(un:Unidad)-[:DEL_IDIOMA]->(idI)  
RETURN DISTINCT e.idEjercicio AS ejercicio,  
    e.nombre AS nombreEjercicio,  
    h.nombre AS habilidad,  
    f.vecesFalladas AS vecesFalladas,  
    un.nombre AS unidad  
ORDER BY f.vecesFalladas DESC; 
``` 