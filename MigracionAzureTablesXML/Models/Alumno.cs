using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracionAzureTablesXML.Models
{
    public class Alumno : ITableEntity
    {
        private string _IdAlumno;
        //ROWKEY
        public string IdAlumno
        {
            get { return this._IdAlumno; }
            set
            {
                this._IdAlumno = value;
                this.RowKey = value;
            }
        }

        private string _Curso;
        //PARTITIONKEY
        public string Curso
        {
            get { return this._Curso; }
            set 
            { 
            this._Curso = value;
            this.PartitionKey= value;
            }
        }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int Nota { get; set; }
        //PARTE DE AZURE
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
