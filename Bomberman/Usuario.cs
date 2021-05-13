using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman
{
    class Usuario
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
    }
}
