using Azure.Data.Tables;
using MigracionAzureTablesXML.Models;
using System.Xml.Linq;


namespace MigracionAzureTablesXML
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void botonMigrar_Click(object sender, EventArgs e)
        {
            //Comenzamos recuperando el documento xml incrustado.
            //Para ello, debemos acceder al assembly y nos devolvera un Strem
            //El doc tendra el namespace del nombre d enuestr poreoct y el nombre del fichero MigracionAzureTablesXML.alumnos.xml
            //DIFERENCIA MAYUS DE MINUS
            string assemblypath = "MigracionAzureTablesXML.alumnos.xml";
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(assemblypath);//

            //Se utiliza Load con flujos de datos (stream) y Parse con strings.
            XDocument document = XDocument.Load(stream);
            //Un documento XML diferencia mayusculas de minusculas.
            //Se recuperan sus elementos por nodos, es decir, por un conjunto de etiquetas <element>, que pueden tener mas etiquetas en su interior
            //Con Descendants ("TAG") indicamos el grupo de etiquetas que deseamos recuperar (alumno en este caso)
            //Se recorre con linq (var consulta = from datos...). Datos es cada element DENTRO de la etiqueta.
            //Para coger los datos de la etiqueta: datos.Element("tag").Value ("tag" es el nombre de la etiqueta dentro de la etiqueta que hemos cogido)


            //*************MANERA MANUAL**********
            //var consulta = from datos in document.Descendants("alumno") select datos; //Coge todas las etiquetas que sean alumno
            //List<Alumno> alumnos = new List<Alumno>();
            ////Recorremos todas las etiquetas alumno
            //foreach(var item in consulta) //item = alumno
            //{
            //    Alumno alumno = new Alumno();
            //    alumno.IdAlumno = item.Element("idalumno").Value;
            //    alumno.Nombre = item.Element("nombre").Value;
            //    alumno.Apellidos = item.Element("apellidos").Value;
            //    alumno.Curso = item.Element("curso").Value;
            //    alumno.Nota = int.Parse(item.Element("nota").Value);

            //    alumnos.Add(alumno);
            //}

            //*************MANERA MANUAL********** Consulta + Clases en la propia consulta
            var consulta = from datos in document.Descendants("alumno")
                           select new Alumno//por cada dato hara un alumno
                           {
                               IdAlumno = datos.Element("idalumno").Value,
                               Nombre = datos.Element("nombre").Value,
                               Apellidos=  datos.Element("apellidos").Value,
                               Curso = datos.Element("curso").Value,
                               Nota =  int.Parse(datos.Element("nota").Value)
                           };
            //Ahora mismo consulta es una coleccion de alumnos
            List<Alumno> alumnos = consulta.ToList();

            //No vamos a crear un service ni nada ya que esta app es una utilidad para rellenar una table de Azure y hacer la practica real con azure
            //AZURE
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=storageeoi0alex;AccountKey=mXDHIsMczC/yrksG/lT4xAPBrISjRbolql9mKKzCPjsNw1RHcsI0LuNuNJEBN5HR+PyUIdCvSPr9+AStsOWEIw==;EndpointSuffix=core.windows.net";
            //EMULADOR
          //  string connectionString = "UseDevelopmentStorage=true";
            //Creamos un servicio de Azure Tables
            TableServiceClient serviceClient = new TableServiceClient(connectionString);
            //Creamos el acceso a la tabla
            TableClient tableClient = serviceClient.GetTableClient("alumnos"); //se va a llamar "alumnos" en azure
            //Creamos la tabla en Azure
            await tableClient.CreateIfNotExistsAsync();
            //Recorremos todos los alumnos que deseamos llevar a Azure
            foreach(Alumno alumno in alumnos)
            {
                //Insertamos cada alumno en Azure
                await tableClient.AddEntityAsync<Alumno>(alumno);
            }
            //Para comprobar si funciona, lo dibujamos en nuestra app
            this.dataGridView1.DataSource = alumnos;
        }
    }
}