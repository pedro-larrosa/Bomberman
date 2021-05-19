using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman
{
    class Usuario : IComparable<Usuario>
    {
        string nombre;
        int puntuacion;
        DateTime fecha;

        public Usuario(string nombre, int puntuacion, DateTime fecha)
        {
            this.nombre = nombre;
            this.puntuacion = puntuacion;
            this.fecha = fecha;
        }

        public int CompareTo(Usuario u2)
        {
            return puntuacion > u2.GetPuntuacion() ? -1 : 
                puntuacion < u2.GetPuntuacion() ? 1 : 0;
        }

        public string GetNombre()
        {
            return nombre;
        }

        public int GetPuntuacion()
        {
            return puntuacion;
        }

        public DateTime GetFecha()
        {
            return fecha;
        }
    }
}
