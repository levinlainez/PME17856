using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace PM2E17858.Models
{
    public class Examen
    {
        [PrimaryKey, AutoIncrement]
        public int codigo { get; set; }
        [MaxLength(100)]
        public string latitude { get; set; }
        [MaxLength(100)]
        public string longitude { get; set; }
        [MaxLength(100)]
        public string descripcion { get; set; }
        public Byte[] foto { get; set; }

    }
}
