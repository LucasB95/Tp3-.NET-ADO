using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
//using Microsoft.Data.SqlClient;


namespace Tp3
{
    class AgenciaManager
    {
        public Agencia miAgencia;
        public List<Usuario> misUsuarios;
        public List<Reserva> misReservas;
        public List<Reserva> misReservasLocal;
        public List<Alojamiento> misAlojamientosAgencia;
        public int contInsertar = 0;
        public int prueba = 0;
        public int alojcont = 0;
        public int usuarioscont = 0;
        public int contReservas = 0;


        public AgenciaManager()
        {
             miAgencia = new Agencia();
             misUsuarios = new List<Usuario> { };
             misReservas = new List<Reserva> { };
             misReservasLocal = new List<Reserva> { };
             misAlojamientosAgencia = new List<Alojamiento> { };
             misAlojamientosAgencia = miAgencia.misAlojamientos;
             misReservasLocal = misReservas;
             inicializarAtributosUsuario();
             inicializarAtributosAlojamiento();
            inicializarAtributosReserva();

        }
        public List<List<string>> buscarAlojamiento(String Ciudad, DateTime Pdesde, DateTime Phasta, int cantPersonas, String Tipo,int estrellas)
        {
            List<List<string>> aloj = new List<List<string>>();
            foreach (Alojamiento u in misAlojamientosAgencia)
            {

                if (u.getCantPersonas() >= cantPersonas || u.getEstrellas() >= estrellas)
                {

                    if (u.getTipo() == Tipo)
                    {
                        aloj.Add(new List<string>() {u.tipo,u.codigo.ToString(), u.nombre, u.ciudad, u.barrio, u.estrellas.ToString(), u.cantPersonas.ToString(),u.tv.ToString(),u.precioDia.ToString(),
                                            u.precioPorPersona.ToString(),u.habitaciones.ToString(),u.baños.ToString()});
                    }
                    else if (Tipo == "Ambos")
                    {
                        aloj.Add(new List<string>() {u.tipo,u.codigo.ToString(), u.nombre, u.ciudad, u.barrio, u.estrellas.ToString(), u.cantPersonas.ToString(),u.tv.ToString(),u.precioDia.ToString(),
                                            u.precioPorPersona.ToString(),u.habitaciones.ToString(),u.baños.ToString()});
                    }

                }

            }

            return aloj;
        }

        private void inicializarAtributosUsuario()
        {
            //Cargo la cadena de conexión desde el archivo de properties
            string connectionString = Properties.Resources.ConnectionString;


            //Defino el string con la consulta que quiero realizar
            string queryString = "SELECT * from dbo.USUARIO";
            //string recorrido = "SELECT COUNT(DNI) FROM dbo.USUARIO";
     

            // Creo una conexión SQL con un Using, de modo que al finalizar, la conexión se cierra y se liberan recursos
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                // Defino el comando a enviar al motor SQL con la consulta y la conexión
                SqlCommand command = new SqlCommand(queryString, connection);
                //SqlCommand tamanio = new SqlCommand(recorrido, connection);

                try
                {
                    //Abro la conexión
                    connection.Open();
                    //mi objecto DataReader va a obtener los resultados de la consulta, notar que a comando se le pide ExecuteReader()
                    SqlDataReader reader = command.ExecuteReader();
                    //SqlDataReader reader1 = tamanio.ExecuteReader();
                    Usuario aux;
                    //mientras haya registros/filas en mi DataReader, sigo leyendo
                    while (reader.Read())
                    {
                                          
                        aux = new Usuario(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetBoolean(4), reader.GetBoolean(5));
                        misUsuarios.Add(aux);
                        prueba++;
                        usuarioscont = misUsuarios.Count();


                    }
                    //En este punto ya recorrí todas las filas del resultado de la query
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public List<List<string>> obtenerUsuarios()
        {
            List<List<string>> salida = new List<List<string>>();
            foreach (Usuario u in misUsuarios)
                salida.Add(new List<string>() { u.DNI.ToString(), u.Nombre, u.Mail, u.Password, u.esAdmin.ToString(), u.bloqueado.ToString() });
            return salida;
        }

        public bool AgregarUsuario(Usuario usu)
        {

            foreach (Usuario a in misUsuarios)
            {


                if (misUsuarios.Exists(x => x.getDNI() == usu.getDNI()))
                {

                    return false;
                }

                else
                {
                    //primero me aseguro que lo pueda agregar a la base
                    int resultadoQuery;
                    string connectionString = Properties.Resources.ConnectionString;
                    string queryString = "INSERT INTO [dbo].[USUARIO] ([DNI],[NOMBRE],[MAIL],[PASSWORD],[ESADMIN],[BLOQUEADO]) VALUES (@dni,@nombre,@mail,@password,@esadm,@bloqueado);";
                    using (SqlConnection connection =
                        new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(queryString, connection);
                        command.Parameters.Add(new SqlParameter("@dni", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@nombre", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@mail", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@esadm", SqlDbType.Bit));
                        command.Parameters.Add(new SqlParameter("@bloqueado", SqlDbType.Bit));
                        command.Parameters["@dni"].Value = usu.getDNI();
                        command.Parameters["@nombre"].Value = usu.getNombre();
                        command.Parameters["@mail"].Value = usu.getMail();
                        command.Parameters["@password"].Value = usu.getPassword();
                        command.Parameters["@esadm"].Value = usu.getesAdmin();
                        command.Parameters["@bloqueado"].Value = usu.getBloqueado();
                        try
                        {
                            connection.Open();
                            //esta consulta NO espera un resultado para leer, es del tipo NON Query
                            resultadoQuery = command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return false;
                        }
                    }
                    if (resultadoQuery == 1)
                    {
                        //Ahora sí lo agrego en la lista
                        misUsuarios.Add(usu);
                        return true;
                    }
                    else
                    {
                        //algo salió mal con la query porque no generó 1 registro
                        return false;
                    }
                }

            }
            return false; 

        }

        public bool eliminarUsuario(int Dni, string Nombre, string Mail, string Password, bool EsADM, bool Bloqueado)
        {
            //primero me aseguro que lo pueda agregar a la base
            int resultadoQuery;
            string connectionString = Properties.Resources.ConnectionString;
            string queryString = "DELETE FROM [dbo].[Usuario] WHERE DNI=@dni AND NOMBRE=@nombre AND MAIL=@mail AND PASSWORD=@password AND ESADMIN=@esadm AND BLOQUEADO=@bloqueado;";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@dni", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@nombre", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@mail", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@esadm", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@bloqueado", SqlDbType.Bit));
                command.Parameters["@dni"].Value = Dni;
                command.Parameters["@nombre"].Value = Nombre;
                command.Parameters["@mail"].Value = Mail;
                command.Parameters["@password"].Value = Password;
                command.Parameters["@esadm"].Value = EsADM;
                command.Parameters["@bloqueado"].Value = Bloqueado;
                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    resultadoQuery = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            if (resultadoQuery == 1)
            {
                try
                {
                    //Ahora sí lo elimino en la lista
                    for (int i = 0; i < misUsuarios.Count; i++)
                        if (misUsuarios[i].DNI == Dni)
                            misUsuarios.RemoveAt(i);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                //algo salió mal con la query porque no generó 1 registro
                return false;
            }
        }

        public List<Usuario> getUsuarios()
        {
            return misUsuarios;
        }

        public bool modificaUsuario(int dni, string passv, string passn, string passnc){

            //Usuario nuevo;
            foreach (Usuario a in misUsuarios)
            {
                if (a.getDNI() == dni && a.getPassword() == passv)
                {
                    if (passn == passnc)
                    {

                        int resultadoQuery;
                        string connectionString = Properties.Resources.ConnectionString;
                        string queryString = "UPDATE [dbo].[Usuario] SET NOMBRE=@nombre, MAIL=@mail,PASSWORD=@password, ESADMIN=@esadm, BLOQUEADO=@bloqueado WHERE DNI=@dni;";
                        using (SqlConnection connection =
                            new SqlConnection(connectionString))
                        {
                            SqlCommand command = new SqlCommand(queryString, connection);
                            command.Parameters.Add(new SqlParameter("@dni", SqlDbType.Int));
                            command.Parameters.Add(new SqlParameter("@nombre", SqlDbType.NVarChar));
                            command.Parameters.Add(new SqlParameter("@mail", SqlDbType.NVarChar));
                            command.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar));
                            command.Parameters.Add(new SqlParameter("@esadm", SqlDbType.Bit));
                            command.Parameters.Add(new SqlParameter("@bloqueado", SqlDbType.Bit));
                            command.Parameters["@dni"].Value = dni;
                            command.Parameters["@nombre"].Value = a.getNombre();
                            command.Parameters["@mail"].Value = a.getMail();
                            command.Parameters["@password"].Value = passn;
                            command.Parameters["@esadm"].Value = a.getesAdmin();
                            command.Parameters["@bloqueado"].Value = a.getBloqueado();
                            try
                            {
                                connection.Open();
                                //esta consulta NO espera un resultado para leer, es del tipo NON Query
                                resultadoQuery = command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                return false;
                            }
                        }
                        if (resultadoQuery == 1)
                        {
                            try
                            {
                                //Ahora sí lo MODIFICO en la lista
                                for (int i = 0; i < misUsuarios.Count; i++)
                                    if (misUsuarios[i].DNI == dni)
                                    {
                                        misUsuarios[i].Nombre = a.getNombre();
                                        misUsuarios[i].Mail = a.getMail();
                                        misUsuarios[i].Password = passn;
                                        misUsuarios[i].esAdmin = a.getesAdmin();
                                        misUsuarios[i].bloqueado = a.getBloqueado();
                                    }
                                return true;
                            }
                            catch (Exception)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            //algo salió mal con la query porque no generó 1 registro
                            return false;
                        }

                    }
                }
            }

                return false;
        
        }
        
        public bool modificarUsuarioAdmin(int Dni, string Nombre, string Mail, string Password, bool EsADM, bool Bloqueado)
        {
            //primero me aseguro que lo pueda agregar a la base
            int resultadoQuery;
            string connectionString = Properties.Resources.ConnectionString;
            string queryString = "UPDATE [dbo].[Usuario] SET NOMBRE=@nombre, MAIL=@mail,PASSWORD=@password, ESADMIN=@esadm, BLOQUEADO=@bloqueado WHERE DNI=@dni;";
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@dni", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@nombre", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@mail", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@esadm", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@bloqueado", SqlDbType.Bit));
                command.Parameters["@dni"].Value = Dni;
                command.Parameters["@nombre"].Value = Nombre;
                command.Parameters["@mail"].Value = Mail;
                command.Parameters["@password"].Value = Password;
                command.Parameters["@esadm"].Value = EsADM;
                command.Parameters["@bloqueado"].Value = Bloqueado;
                try
                {
                    connection.Open();
                    //esta consulta NO espera un resultado para leer, es del tipo NON Query
                    resultadoQuery = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            if (resultadoQuery == 1)
            {
                try
                {
                    //Ahora sí lo MODIFICO en la lista
                    for (int i = 0; i < misUsuarios.Count; i++)
                        if (misUsuarios[i].DNI == Dni)
                        {
                            misUsuarios[i].Nombre = Nombre;
                            misUsuarios[i].Mail = Mail;
                            misUsuarios[i].Password = Password;
                            misUsuarios[i].esAdmin = EsADM;
                            misUsuarios[i].bloqueado = Bloqueado;
                        }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                //algo salió mal con la query porque no generó 1 registro
                return false;
            }
        }

        public bool autenticarUsuario(int DNI, string password)
        {


            for (int i = 0; i < misUsuarios.Count(); i++)
            {
                if (misUsuarios[i].getDNI() == DNI && misUsuarios[i].getPassword() == password) 
                {
                    if (!misUsuarios[i].getBloqueado())
                    {
                    return true;

                    }
                }
       
            }
      
            return false;
        }
        public bool autenticarUsuarioAdmin(int DNI, string password) {

            for (int i = 0; i < misUsuarios.Count(); i++)
            {
                if (misUsuarios[i].getDNI() == DNI && misUsuarios[i].getPassword() == password)
                {
                    if (misUsuarios[i].getesAdmin())
                    {
                    return true;
                    }
                }
            }
         
            return false;
        }

        public bool desbloquearUsuario(Usuario usu)
        {
            foreach (Usuario a in misUsuarios)
            {
                if (a != null && a.getBloqueado() != true)
                {
                    usu.setBloqueado(true);
                    return true;
                }
            }
            return false;
        }

        public bool bloquearUsuario(int dni)
        {
            foreach (Usuario a in misUsuarios)
            {
                if (a != null && a.getDNI() == dni)
                {
                    misUsuarios.Remove(a);
                    a.setBloqueado(false);
                    misUsuarios.Add(a);
                    return true;
                }
            }
            return false;
        }

        private void inicializarAtributosAlojamiento()
        {
            
            string connectionString = Properties.Resources.ConnectionString;

            string queryString = "SELECT * from dbo.ALOJAMIENTO";
   
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
       
                try
                {
                   
                SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();                    
                    SqlDataReader reader = command.ExecuteReader();

                    Alojamiento alojamiento;
                    
                    while (reader.Read())
                    {

                       
                        alojamiento = new Alojamiento(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetBoolean(6),
                         reader.GetInt32(7), reader.GetInt32(8), reader.GetInt32(9), reader.GetInt32(10), reader.GetString(11));

                        
                        //misAlojamientosAgencia.Add(alojamiento);
                        miAgencia.insertarAlojamiento(alojamiento);
                                    
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public List<List<string>> obtenerAlojamiento()
        {
           
            List<List<string>> aloj = new List<List<string>>();

            foreach (Alojamiento u in miAgencia.misAlojamientos)
                aloj.Add(new List<string>() {u.tipo,u.codigo.ToString(), u.nombre, u.ciudad, u.barrio, u.estrellas.ToString(), u.cantPersonas.ToString(),u.tv.ToString(),u.precioDia.ToString(),
                                            u.precioPorPersona.ToString(),u.habitaciones.ToString(),u.baños.ToString()});
            return aloj;
        }

        public bool agregarAlojamiento(Alojamiento aloj)
        {
            foreach (Alojamiento a in misAlojamientosAgencia)
            {
                if (misAlojamientosAgencia.Exists(x => x.getCodigo() == a.getCodigo()))
                {
                    return false;
                }
                else
                {

                    alojcont++;
                    //primero me aseguro que lo pueda agregar a la base
                    int resultadoQuery;
                    string connectionString = Properties.Resources.ConnectionString;
                    string queryString = "INSERT INTO [dbo].[ALOJAMIENTO] ([CODIGO],[TIPO],[BARRIO],[CIUDAD],[ESTRELLAS],[CANTPERSONAS],[TV],[PRECIODIA_CABAÑA],[PRECIOPERSONA_HOTEL],[HABITACIONES],[BAÑOS],[NOMBRE])  VALUES (@codigo,@tipo,@barrio,@ciudad,@estrellas,@cantpersonas,@tv,@preciodia_cabaña,@preciopersona_hotel,@habitaciones,@baños,@nombre);";
                    using (SqlConnection connection =
                        new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(queryString, connection);
                        command.Parameters.Add(new SqlParameter("@codigo", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@tipo", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@barrio", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@ciudad", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@estrellas", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@cantpersonas", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@tv", SqlDbType.Bit));
                        command.Parameters.Add(new SqlParameter("@preciodia_cabaña", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@preciopersona_hotel", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@habitaciones", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@baños", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@nombre", SqlDbType.NVarChar));
                        command.Parameters["@codigo"].Value = aloj.getCodigo();
                        command.Parameters["@tipo"].Value = aloj.getTipo();
                        command.Parameters["@barrio"].Value = aloj.getBarrio();
                        command.Parameters["@ciudad"].Value = aloj.getCiudad();
                        command.Parameters["@estrellas"].Value = aloj.getEstrellas();
                        command.Parameters["@tv"].Value = aloj.getTV();
                        command.Parameters["@preciodia_cabaña"].Value = aloj.getPrecioDia();
                        command.Parameters["@preciopersona_hotel"].Value = aloj.getPrecioPorPersona();
                        command.Parameters["@habitaciones"].Value = aloj.getHabitaciones();
                        command.Parameters["@baños"].Value = aloj.getbaños();
                        command.Parameters["@nombre"].Value = aloj.getNombre();
                        try
                        {
                            connection.Open();
                            //esta consulta NO espera un resultado para leer, es del tipo NON Query
                            resultadoQuery = command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return false;
                        }
                    }
                    if (resultadoQuery == 1)
                    {
                        //Ahora sí lo agrego en la lista
                        miAgencia.insertarAlojamiento(aloj);
                        return true;
                    }
                    else
                    {
                        //algo salió mal con la query porque no generó 1 registro
                        return false;
                    }
                }
            }
            return false;
        }

        //public bool agregarAlojamiento(Alojamiento aloj)
        //{
        //    if (miAgencia.insertarAlojamiento(aloj))
        //    {
        //        alojcont++;
        //        return true;
        //    }
        //    return false;
        //}

        public bool modificarAlojamiento(Alojamiento aloj)
        {

            if (miAgencia.modificarAlojamiento(aloj))
            {
                //primero me aseguro que lo pueda agregar a la base
                int resultadoQuery;
                string connectionString = Properties.Resources.ConnectionString;
                string queryString = "UPDATE [dbo].[ALOJAMIENTO] SET CODIGO=@codigo,TIPO=@tipo,BARRIO=@barrio,CIUDAD=@ciudad,ESTRELLAS=@estrellas,CANTPERSONAS=@cantpersonas,TV=@tv,PRECIODIA_CABAÑA=@preciodia_cabaña,PRECIOPERSONA_HOTEL=@preciopersona_hotel,HABITACIONES=@habitaciones,BAÑOS=@baños,NOMBRE=@nombre WHERE CODIGO=@codigo;";
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add(new SqlParameter("@codigo", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@tipo", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@barrio", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@ciudad", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@estrellas", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@cantpersonas", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@tv", SqlDbType.Bit));
                    command.Parameters.Add(new SqlParameter("@preciodia_cabaña", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@preciopersona_hotel", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@habitaciones", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@baños", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@nombre", SqlDbType.NVarChar));
                    command.Parameters["@codigo"].Value = aloj.getCodigo();
                    command.Parameters["@tipo"].Value = aloj.getTipo();
                    command.Parameters["@barrio"].Value = aloj.getBarrio();
                    command.Parameters["@ciudad"].Value = aloj.getCiudad();
                    command.Parameters["@estrellas"].Value = aloj.getEstrellas();
                    command.Parameters["@tv"].Value = aloj.getTV();
                    command.Parameters["@preciodia_cabaña"].Value = aloj.getPrecioDia();
                    command.Parameters["@preciopersona_hotel"].Value = aloj.getPrecioPorPersona();
                    command.Parameters["@habitaciones"].Value = aloj.getHabitaciones();
                    command.Parameters["@baños"].Value = aloj.getbaños();
                    command.Parameters["@nombre"].Value = aloj.getNombre();
                    try
                    {
                        connection.Open();
                        //esta consulta NO espera un resultado para leer, es del tipo NON Query
                        resultadoQuery = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }
                if (resultadoQuery == 1)
                {
                    try
                    {
                        //Ahora sí lo MODIFICO en la lista
                        for (int i = 0; i < miAgencia.misAlojamientos.Count(); i++)
                            if (miAgencia.misAlojamientos[i].codigo == aloj.getCodigo())
                            {
                                miAgencia.misAlojamientos[i].tipo = aloj.getTipo();
                                miAgencia.misAlojamientos[i].barrio = aloj.getBarrio();
                                miAgencia.misAlojamientos[i].ciudad = aloj.getCiudad();
                                miAgencia.misAlojamientos[i].estrellas = aloj.getEstrellas();
                                miAgencia.misAlojamientos[i].cantPersonas = aloj.getCantPersonas();
                                miAgencia.misAlojamientos[i].tv = aloj.getTV();
                                miAgencia.misAlojamientos[i].precioDia = aloj.getPrecioDia();
                                miAgencia.misAlojamientos[i].precioPorPersona = aloj.getPrecioPorPersona();
                                miAgencia.misAlojamientos[i].habitaciones = aloj.getHabitaciones();
                                miAgencia.misAlojamientos[i].baños = aloj.getbaños();
                                miAgencia.misAlojamientos[i].nombre = aloj.getNombre();

                            }
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                else
                {
                    //algo salió mal con la query porque no generó 1 registro
                    return false;
                }
            }
            return false;
        }

        public bool quitarAlojamiento(Alojamiento aloj)
        {
            if (miAgencia.eliminarAlojamiento(aloj))
            {
                //primero me aseguro que lo pueda agregar a la base
                int resultadoQuery;
                string connectionString = Properties.Resources.ConnectionString;
                string queryString = "DELETE FROM [dbo].[ALOJAMIENTO] WHERE CODIGO=@codigo  AND TIPO=@tipo AND BARRIO=@barrio AND CIUDAD=@ciudad AND ESTRELLAS=@estrellas AND CANTPERSONAS=@cantpersonasADN TV=@tv AND PRECIODIA_CABAÑA=@preciodia_cabaña AND PRECIOPERSONA_HOTEL=@preciopersona_hotel AND HABITACIONES=@habitaciones AND BAÑOS=@baños AND NOMBRE=@nombre";
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add(new SqlParameter("@codigo", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@tipo", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@barrio", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@ciudad", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@estrellas", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@cantpersonas", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@tv", SqlDbType.Bit));
                    command.Parameters.Add(new SqlParameter("@preciodia_cabaña", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@preciopersona_hotel", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@habitaciones", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@baños", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@nombre", SqlDbType.NVarChar));
                    command.Parameters["@codigo"].Value = aloj.getCodigo();
                    command.Parameters["@tipo"].Value = aloj.getTipo();
                    command.Parameters["@barrio"].Value = aloj.getBarrio();
                    command.Parameters["@ciudad"].Value = aloj.getCiudad();
                    command.Parameters["@estrellas"].Value = aloj.getEstrellas();
                    command.Parameters["@tv"].Value = aloj.getTV();
                    command.Parameters["@preciodia_cabaña"].Value = aloj.getPrecioDia();
                    command.Parameters["@preciopersona_hotel"].Value = aloj.getPrecioPorPersona();
                    command.Parameters["@habitaciones"].Value = aloj.getHabitaciones();
                    command.Parameters["@baños"].Value = aloj.getbaños();
                    command.Parameters["@nombre"].Value = aloj.getNombre();
                    try
                    {
                        connection.Open();
                        //esta consulta NO espera un resultado para leer, es del tipo NON Query
                        resultadoQuery = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }
                if (resultadoQuery == 1)
                {
                    try
                    {
                        //Ahora sí lo elimino en la lista
                        for (int i = 0; i < miAgencia.misAlojamientos.Count(); i++)
                            if (miAgencia.misAlojamientos[i].codigo == aloj.getCodigo())
                            {
                                miAgencia.misAlojamientos.RemoveAt(i);
                                return true;
                            }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                else
                {
                    //algo salió mal con la query porque no generó 1 registro
                    return false;
                }
            }
            return false;
        }

        private void inicializarAtributosReserva()
        {

            string connectionString = Properties.Resources.ConnectionString;

            string queryString = "SELECT * from dbo.RESERVA";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {

                try
                {

                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    Reserva reserva;

                    while (reader.Read())
                    {


                        reserva = new Reserva(reader.GetInt32(0), reader.GetDateTime(1), reader.GetDateTime(2), reader.GetInt32(3), reader.GetInt32(4),reader.GetInt32(5));

                        //misAlojamientosAgencia.Add(alojamiento);
                        misReservas.Add(reserva);
                        contReservas++;

                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public List<List<string>> obtenerReserva()
        {
            List<List<string>> salida = new List<List<string>>();
            foreach (Reserva u in misReservas)
             
                salida.Add(new List<string>() { u.id.ToString(), u.fdesde.ToString(), u.fhasta.ToString(), u.precio.ToString(), u.propiedadint.ToString(), u.personaint.ToString() });
            return salida;
        }

        public List<List<string>> obtenerReservas(int dni )
        {
            List<List<string>> salida = new List<List<string>>();
            foreach (Reserva u in misReservas)
            {
                if (u.personaint == dni)
                {
                    salida.Add(new List<string>() { u.id.ToString(), u.fdesde.ToString(), u.fhasta.ToString(), u.precio.ToString(), u.propiedadint.ToString(), u.personaint.ToString() });

                }
            }
            return salida;
        }

        public List<Usuario> buscarReserva(int DNIusuario)
        {
            List<Usuario> usuarios = new List<Usuario> { };
            foreach (Usuario a in misUsuarios)
            {
                if (a.getDNI() == DNIusuario)
                {
                    usuarios.Add(a);
                }
            }
            return usuarios;
        }

        public bool reservar(Reserva reservar)
        {

            bool reserva = true;
            foreach (Reserva reser in misReservas)
            {
                if ((reser.getFDesde() == reservar.getFDesde()) && (reser.getFHasta() == reservar.getFHasta()))
                {
                    reserva = false;
                }
                else
                {
                    //primero me aseguro que lo pueda agregar a la base
                    int resultadoQuery;
                    string connectionString = Properties.Resources.ConnectionString;
                    string queryString = "INSERT INTO [dbo].[RESERVA] ([ID],[FDESDE],[FHASTA],[PRECIO],[ID_ALOJAMIENTO],[DNI_USUARIO]) VALUES (@id,@fdesde,@fhasta,@precio,@id_alojamiento,@dni_usuario);";
                    using (SqlConnection connection =
                        new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(queryString, connection);
                        command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@fdesde", SqlDbType.DateTime));
                        command.Parameters.Add(new SqlParameter("@fhasta", SqlDbType.DateTime));
                        command.Parameters.Add(new SqlParameter("@precio", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@id_alojamiento", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@dni_usuario", SqlDbType.Int));
                        command.Parameters["@id"].Value = reservar.getID();
                        command.Parameters["@fdesde"].Value = reservar.getFDesde();
                        command.Parameters["@fhasta"].Value = reservar.getFHasta();
                        command.Parameters["@precio"].Value = reservar.getPrecio();
                        command.Parameters["@id_alojamiento"].Value = reservar.getPropiedadInt();
                        command.Parameters["@dni_usuario"].Value = reservar.getPersonaint();
                        try
                        {
                            connection.Open();
                            //esta consulta NO espera un resultado para leer, es del tipo NON Query
                            resultadoQuery = command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return false;
                        }
                    }
                    if (resultadoQuery == 1)
                    {
                        //Ahora sí lo agrego en la lista
                        misReservas.Add(reservar);
                        return true;
                    }
                    else
                    {
                        //algo salió mal con la query porque no generó 1 registro
                        return false;
                    }                  
             
                }

            }
           

            return reserva;

        }

        public bool modificarReserva(Reserva reservaNueva, int idAModificar) // Datos de Reserva
        {
            bool reserv = false;
            foreach (Reserva reser in misReservas)
            {
                if (idAModificar == reser.getID())
                {
                    misReservas.Remove(reser);
                    misReservas.Add(reservaNueva);
                    reserv = true;
                }
            }
            return reserv;
        }

        public bool eliminarReserva(Reserva reservar)
        {
            foreach (Reserva reser in misReservas)
            {
                if (reser.getID() == reservar.getID())
                {
                    //primero me aseguro que lo pueda agregar a la base
                    int resultadoQuery;
                    string connectionString = Properties.Resources.ConnectionString;
                    string queryString = "DELETE FROM [dbo].[RESERVA] WHERE ID=@id AND FDESDE=@fdesde AND FHASTA=@fhasta AND PRECIO=@precio AND ID_ALOJAMIENTO=@id_alojamiento AND DNI_USUARIO=@dni_usuario;";
                    using (SqlConnection connection =
                        new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(queryString, connection);
                        command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@fdesde", SqlDbType.DateTime));
                        command.Parameters.Add(new SqlParameter("@fhasta", SqlDbType.DateTime));
                        command.Parameters.Add(new SqlParameter("@precio", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@id_alojamiento", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@dni_usuario", SqlDbType.Int));
                        command.Parameters["@id"].Value = reservar.getID();
                        command.Parameters["@fdesde"].Value = reservar.getFDesde();
                        command.Parameters["@fhasta"].Value = reservar.getFHasta();
                        command.Parameters["@precio"].Value = reservar.getPrecio();
                        command.Parameters["@id_alojamiento"].Value = reservar.getPropiedadInt();
                        command.Parameters["@dni_usuario"].Value = reservar.getPersonaint();
                        try
                        {
                            connection.Open();
                            //esta consulta NO espera un resultado para leer, es del tipo NON Query
                            resultadoQuery = command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return false;
                        }
                    }
                    if (resultadoQuery == 1)
                    {
                        try
                        {
                            //Ahora sí lo elimino en la lista
                            for (int i = 0; i < misReservas.Count; i++)
                                if (misReservas[i].id == reservar.id)
                                    misReservas.RemoveAt(i);
                            return true;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //algo salió mal con la query porque no generó 1 registro
                        return false;
                    }
                }


            }
            return false;

        }




    }
}
