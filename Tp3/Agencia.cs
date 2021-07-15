using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Tp3
{
    class Agencia
    {
        public List<Alojamiento> misAlojamientos;
        public int cantAlojamientos; 


        public Agencia()
        {
            misAlojamientos = new List<Alojamiento>();
           
        }

        public Agencia(int CantidadAlojamientos)
        {
            misAlojamientos = new List<Alojamiento>();
            cantAlojamientos = CantidadAlojamientos;
        }

        public bool insertarAlojamiento(Alojamiento aloj)
        {
            foreach (Alojamiento a in misAlojamientos)

                if (a != null && a.igualCodigo(aloj))

                    return false;

            misAlojamientos.Add(aloj);
            return true;
        }
           

        public bool estaAlojamiento(Alojamiento aloj)
        {
            foreach (Alojamiento a in misAlojamientos)
                if (a.igualCodigo(aloj))
                    return true;

            return false;
        }



        public bool eliminarAlojamiento(Alojamiento aloj)
        {
            foreach (Alojamiento a in misAlojamientos)
            {
                if (a.igualCodigo(aloj))
                {
                    misAlojamientos.Remove(aloj);
                    return true;
                }
            }
            return false;
        }

        public bool modificarAlojamiento(Alojamiento aloj)
        {
            foreach (Alojamiento a in misAlojamientos)
            {
                if (a.igualCodigo(aloj))
                {
                    misAlojamientos.Remove(a);
                    misAlojamientos.Add(aloj);

                }
                return true;
            }
            return false;
        }

        public List<Alojamiento> getAloj()
        {
            return misAlojamientos;
        }
       

        public bool estaLlena() { return cantAlojamientos == misAlojamientos.Count; }
        public bool hayAlojamientos() { return misAlojamientos.Count > 0; }

        public Agencia masEstrellas(int cant)
        {
            Agencia Salida = new Agencia(this.cantAlojamientos);
            foreach (Alojamiento a in misAlojamientos)
                if (a.getEstrellas() >= cant)
                    Salida.insertarAlojamiento(a);
            return Salida;
        }

        //public Agencia cabañasEntrePrecios(float d, float h)
        //{
        //    Agencia Salida = new Agencia(this.cantAlojamientos);
        //    foreach (Alojamiento a in misAlojamientos)
        //        if (a is Cabaña)
        //        {
        //            Cabaña c = (Cabaña)a;
        //            if (c.getPrecioPorPersona() <= h && c.getPrecioPorPersona() >= d)
        //                Salida.insertarAlojamiento(c);
        //        }

        //    return Salida;
        //}


        // esto antes no funcionaba esperando respuesta del profe
        //public Agencia alojamientosEntrePrecios(float d, float h)
        //{
        //    Agencia Salida = new Agencia(this.cantAlojamientos);

        //    foreach (Alojamiento a in misAlojamientos)
        //        if (a is Cabaña)
        //        {
        //            Cabaña c = (Cabaña)a;
        //            if (c.getPrecioPorPersona() <= h && c.getPrecioPorPersona() >= d)
        //                Salida.insertarAlojamiento(c);
        //            Console.WriteLine(c.ToString());
        //        }
        //        else if (a is Hotel)
        //        {
        //            Hotel t = (Hotel)a;
        //            if (t.getPrecioPorPersona() <= h && t.getPrecioPorPersona() >= d)
        //                Salida.insertarAlojamiento(t);
        //            Console.WriteLine(t.ToString());
        //        }

        //    return Salida;
        //}


        public int getCantidad() { return cantAlojamientos; }
        public void setCantidad(int CantAlojamientos) { cantAlojamientos = CantAlojamientos; }

        public List<Alojamiento> getAlojamientos()
        {
            return misAlojamientos;
            //return misAlojamientos.OrderBy(a => a.getEstrellas()).ThenBy(a => a.getCantPersonas()).ThenBy(a => a.getCodigo()).ToList();
        }
    }
}
