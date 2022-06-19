using System;
using System.Collections.Generic;
using System.Text;
using PM2E17858.Models;
using SQLite;
using System.Threading.Tasks;

namespace PM2E17858.Controls
{
    public class DateBaseSQLite
    {
        readonly SQLiteAsyncConnection db;


        //constructor de clase
        public DateBaseSQLite(string pathdb)
        {
            //crear la conexion de la base datos
            db = new SQLiteAsyncConnection(pathdb);
            //creacion de la tabla dentro de sqlite
            db.CreateTableAsync<Examen>().Wait();
        }

        //Operaciones CRUD con SQLite
        //READ List Way
        public Task<List<Examen>> ObtenerListaUbicaciones()
        {
            return db.Table<Examen>().ToListAsync();
        }

        //Mostrar solo una persona
        public Task<Examen> ObtenerUbicacion(int pcodigo)
        {
            return db.Table<Examen>()
                .Where(i => i.codigo == pcodigo)
                .FirstOrDefaultAsync();
        }

        //guardar persona
        public Task<int> GuardarUbicacion(Examen ubicacion)
        {
            if (ubicacion.codigo != 0)
            {
                return db.UpdateAsync(ubicacion);
            }
            else
            {
                return db.InsertAsync(ubicacion);
            }



        }
        //Eliminar persona
        public Task<int> EliminarUbicacion(Examen ubicacion)
        {
            return db.DeleteAsync(ubicacion);
        }


    }
}

