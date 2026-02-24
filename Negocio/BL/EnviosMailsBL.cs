using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Configuration;

namespace Negocio
{
    public partial class EnviosMailsBL
    {
        /// <summary>
        /// Se envía un mail a: 
        /// - Todos los usuarios con Rol Confederación excepto el usuario que creó la Federación
        /// </summary>
        public static void notificacionNuevaFederacion(int id, int idUsuario)
        {
            Negocio.Federaciones instancia = Negocio.FederacionesBL.obtener(id);

            string body = String.Format(@"
                <p>Se le notifica que se ha creado una nueva Federación:</p>
                <ul>
                    <li><strong>Nombre: </strong>{0}</li>
                    <li><strong>Dirección: </strong>{1}</li>
                    <li><strong>Localidad: </strong>{2}</li>
                    <li><strong>Teléfono: </strong>{3}</li>
                    <li><strong>Fax: </strong>{4}</li>
                    <li><strong>E-Mail: </strong>{5}</li>
                </ul>
            ", instancia.descripcion
             , instancia.direccion
             , instancia.Localidades != null ? instancia.Localidades.descripcion + (instancia.Localidades.Provincias != null ? " - " + instancia.Localidades.Provincias.descripcion : "") : ""
             , instancia.telefono
             , instancia.fax
             , instancia.mail);

            enviarMail("FUB - Nueva Federación", body, Negocio.UsuariosBL.obtenerUsuariosPorRol("confederacion", idUsuario));
        }

        /// <summary>
        /// Se envía un mail a: 
        /// - Todos los usuarios con Rol Confederación
        /// - Todos los usuarios con Rol Federación asociados a la federación de la nueva asociación excepto el usuario que creó la Asociación
        /// </summary>
        public static void notificacionNuevaAsociacion(int id, int idUsuario)
        {
            Negocio.Asociaciones instancia = Negocio.AsociacionesBL.obtener(id);

            string body = String.Format(@"
                <p>Se le notifica que se ha creado una nueva Asociación:</p>
                <ul>
                    <li><strong>Nombre: </strong>{0}</li>
                    <li><strong>Dirección: </strong>{1}</li>
                    <li><strong>Localidad: </strong>{2}</li>
                    <li><strong>Teléfono: </strong>{3}</li>
                    <li><strong>Fax: </strong>{4}</li>
                    <li><strong>E-Mail: </strong>{5}</li>
                </ul>
            ", instancia.descripcion
             , instancia.direccion
             , instancia.Localidades != null ? instancia.Localidades.descripcion + (instancia.Localidades.Provincias != null ? " - " + instancia.Localidades.Provincias.descripcion : "") : ""
             , instancia.telefono
             , instancia.fax
             , instancia.mail);

            enviarMail("FUB - Nueva Asociación", body, Negocio.UsuariosBL.obtenerUsuariosPorRol("confederacion"));
            enviarMail("FUB - Nueva Asociación", body, Negocio.UsuariosBL.obtenerUsuariosPorRol("federacion", idUsuario, instancia.idFederacion));
        }

        /// <summary>
        /// Se envía un mail a: 
        /// - Todos los usuarios con Rol Confederación
        /// - Todos los usuarios con Rol Federación asociados a la federación de la asociación del neuvo club
        /// - Todos los usuarios con Rol Asociación asociados a la asociación del nuevo club excepto el usuario que creó la Asociación
        /// </summary>
        public static void notificacionNuevoClub(int id, int idUsuario)
        {
            Negocio.Clubes instancia = Negocio.ClubesBL.obtener(id);

            string body = String.Format(@"
                <p>Se le notifica que se ha creado un nuevo Club:</p>
                <ul>
                    <li><strong>Nombre: </strong>{0}</li>
                    <li><strong>Dirección: </strong>{1}</li>
                    <li><strong>Localidad: </strong>{2}</li>
                    <li><strong>Teléfono: </strong>{3}</li>
                    <li><strong>Fax: </strong>{4}</li>
                    <li><strong>E-Mail: </strong>{5}</li>
                </ul>
            ", instancia.descripcion
             , instancia.direccion
             , instancia.Localidades != null ? instancia.Localidades.descripcion + (instancia.Localidades.Provincias != null ? " - " + instancia.Localidades.Provincias.descripcion : "") : ""
             , instancia.telefono
             , instancia.fax
             , instancia.mail);

            enviarMail("FUB - Nuevo Club", body, Negocio.UsuariosBL.obtenerUsuariosPorRol("confederacion"));
            enviarMail("FUB - Nuevo Club", body, Negocio.UsuariosBL.obtenerUsuariosPorRol("federacion", idUsuario, instancia.Asociaciones.idFederacion));
            enviarMail("FUB - Nuevo Club", body, Negocio.UsuariosBL.obtenerUsuariosPorRol("asociacion", idUsuario, instancia.idAsociacion));
        }

        /// <summary>
        /// Se envía un mail a: 
        /// - Todos los usuarios con Rol Confederación
        /// - Todos los usuarios con Rol Federación asociados a la federación de la asociación del club del nuevo jugador
        /// - Todos los usuarios con Rol Asociación asociados a la asociación del club excepto el usuario que creó la Asociación
        /// </summary>
        public static void notificacionNuevoJugador(int id, int idUsuario)
        {
            Negocio.Jugadores instancia = Negocio.JugadoresBL.obtener(id);
            Negocio.Usuarios usuario = Negocio.UsuariosBL.obtener(idUsuario);

            string body = String.Format(@"
                <p>Se le notifica que se ha creado un nuevo Jugador:</p>
                <ul>
                    <li><strong>Nombre: </strong>{0}</li>
                    <li><strong>DNI: </strong>{1}</li>
                    <li><strong>Sexo: </strong>{2}</li>
                    <li><strong>Fecha de Nacimiento: </strong>{3}</li>
                    <li><strong>Categoría: </strong>{4}</li>
                    <li><strong>Dirección: </strong>{5}</li>
                    <li><strong>Localidad: </strong>{6}</li>
                    <li><strong>Teléfono Fijo: </strong>{7}</li>
                    <li><strong>Celular: </strong>{8}</li>
                    <li><strong>E-Mail: </strong>{9}</li>
                    <li><strong>Solicitó Carnet: </strong>{10}</li>
                </ul>   
                <p>Usuario: {11} - {12} - id = {13}. ​</p>
            ", instancia.apellido + " " + instancia.nombre
             , instancia.DNI
             , instancia.sexo
             , instancia.fechaNacimiento.ToShortDateString()
             , instancia.Categorias != null ? instancia.Categorias.descripcion : ""
             , instancia.direccion
             , instancia.Localidades != null ? instancia.Localidades.descripcion + (instancia.Localidades.Provincias != null ? " - " + instancia.Localidades.Provincias.descripcion : "") : instancia.localidad
             , instancia.telefonoFijo
             , instancia.celular
             , instancia.mail
             , instancia.solicitaCarnet == true ? "SI" : "NO"
             , usuario.nombreUsuario
             , usuario.nombre
             , usuario.id);
             
             

            enviarMail("FUB - Nuevo Jugador", body, Negocio.UsuariosBL.obtenerUsuariosPorRol("confederacion"));
            enviarMail("FUB - Nuevo Jugador", body, Negocio.UsuariosBL.obtenerUsuariosPorRol("federacion", idUsuario, instancia.Clubes.Asociaciones.idFederacion));
            enviarMail("FUB - Nuevo Jugador", body, Negocio.UsuariosBL.obtenerUsuariosPorRol("asociacion", idUsuario, instancia.Clubes.idAsociacion));
        }
        public static void notificacionCambioFoto(int id, int idUsuario)
        {
            Negocio.Jugadores instancia = Negocio.JugadoresBL.obtener(id);
            Negocio.Usuarios usuario = Negocio.UsuariosBL.obtener(idUsuario);

            string body = String.Format(@"
                <p>Se le notifica que se ha modificado la foto de un Jugador:</p>
                <ul>
                    <li><strong>Nombre: </strong>{0}</li>
                    <li><strong>DNI: </strong>{1}</li>
                    <li><strong>Sexo: </strong>{2}</li>
                    <li><strong>Fecha de Nacimiento: </strong>{3}</li>
                    <li><strong>Categoría: </strong>{4}</li>
                    <li><strong>Dirección: </strong>{5}</li>
                    <li><strong>Localidad: </strong>{6}</li>
                    <li><strong>Teléfono Fijo: </strong>{7}</li>
                    <li><strong>Celular: </strong>{8}</li>
                    <li><strong>E-Mail: </strong>{9}</li>
                    <li><strong>Solicitó Carnet: </strong>{10}</li>
                </ul>   
                <p>Usuario: {11} - {12} - id = {13}. ​</p>
            ", instancia.apellido + " " + instancia.nombre
             , instancia.DNI
             , instancia.sexo
             , instancia.fechaNacimiento.ToShortDateString()
             , instancia.Categorias != null ? instancia.Categorias.descripcion : ""
             , instancia.direccion
             , instancia.Localidades != null ? instancia.Localidades.descripcion + (instancia.Localidades.Provincias != null ? " - " + instancia.Localidades.Provincias.descripcion : "") : instancia.localidad
             , instancia.telefonoFijo
             , instancia.celular
             , instancia.mail
             , instancia.solicitaCarnet == true ? "SI" : "NO"
             , usuario.nombreUsuario
             , usuario.nombre
             , usuario.id);


            string email = System.Configuration.ConfigurationManager.AppSettings["emailNotificacion"];

            enviarMail("FUB - Cambio de Foto", body, email);
           
        }

        /// <summary>
        /// Se envía un mail a: 
        /// - Todos los usuarios con Rol Confederación
        /// </summary>
        public static void notificacionSolicitudCarnet(int id, int idUsuario)
        {
            Negocio.Jugadores instancia = Negocio.JugadoresBL.obtener(id);
            Negocio.Usuarios usuario = Negocio.UsuariosBL.obtener(idUsuario);

            string body = String.Format(@"
                <p>Se le notifica que se ha solicitado un nuevo carnet para el siguiente Jugador:</p>
                <ul>
                    <li><strong>Nombre: </strong>{0}</li>
                    <li><strong>DNI: </strong>{1}</li>
                    <li><strong>Sexo: </strong>{2}</li>
                    <li><strong>Fecha de Nacimiento: </strong>{3}</li>
                    <li><strong>Categoría: </strong>{4}</li>
                    <li><strong>Dirección: </strong>{5}</li>
                    <li><strong>Localidad: </strong>{6}</li>
                    <li><strong>Teléfono Fijo: </strong>{7}</li>
                    <li><strong>Celular: </strong>{8}</li>
                    <li><strong>E-Mail: </strong>{9}</li>
                </ul>
                <p>Usuario: {10} - {11} - id = {12}. ​</p>
            ", instancia.apellido + " " + instancia.nombre
             , instancia.DNI
             , instancia.sexo
             , instancia.fechaNacimiento.ToShortDateString()
             , instancia.Categorias != null ? instancia.Categorias.descripcion : ""
             , instancia.direccion
             , instancia.Localidades != null ? instancia.Localidades.descripcion + (instancia.Localidades.Provincias != null ? " - " + instancia.Localidades.Provincias.descripcion : "") : instancia.localidad
             , instancia.telefonoFijo
             , instancia.celular
             , instancia.mail
             , usuario.nombreUsuario
             , usuario.nombre
             , usuario.id);

            string email = System.Configuration.ConfigurationManager.AppSettings["emailNotificacion"];

            enviarMail("FUB - Solicitud de Carnet", body, email);
        }

        /// <summary>
        /// Dependiendo del tipo de pase se envía un mail a: 
        /// PASE INTER-FEDERACION
        ///     - Todos los usuarios con Rol Confederación
        /// PASE INTER-ASOCIACIÓN
        ///     - Todos los usuarios con Rol Federación
        /// PASE INTER-CLUB
        /// - Todos los usuarios con Rol Asociación
        /// </summary>
        public static void notificacionPaseParaAprobar(int id, int idUsuario)
        {
            Negocio.JugadoresPases instancia = Negocio.JugadoresPasesBL.obtener(id);
            Negocio.Usuarios usuario = Negocio.UsuariosBL.obtener(idUsuario);


            var roles = Negocio.UsuariosBL.obtenerRol(idUsuario);
            

            //datos del pase
            string body = String.Format(@"
                <p>Se le notifica que se ha creado un nuevo Pase que requiere ser aprobado por Ud:</p>
                <p><strong>Datos del Jugador</strong></p>
                <ul>
                    <li><strong>Nombre: </strong>{0}</li>
                    <li><strong>DNI: </strong>{1}</li>
                    <li><strong>Sexo: </strong>{2}</li>
                    <li><strong>Fecha de Nacimiento: </strong>{3}</li>
                    <li><strong>Categoría: </strong>{4}</li>
                    <li><strong>Dirección: </strong>{5}</li>
                    <li><strong>Localidad: </strong>{6}</li>
                    <li><strong>Teléfono Fijo: </strong>{7}</li>
                    <li><strong>Celular: </strong>{8}</li>
                    <li><strong>E-Mail: </strong>{9}</li>
                </ul>
                <p><strong>Datos del Pase</strong></p>
                <ul>
                    <li><strong>Tipo de Pase: </strong>{17}
                    <li><strong>Club Origen: </strong>{10}
                    <li><strong>Asociación Origen: </strong>{11}
                    <li><strong>Federación Origen: </strong>{12}
                    <li><strong>Club Destino: </strong>{13}
                    <li><strong>Asociación Destino: </strong>{14}
                    <li><strong>Federación Destino: </strong>{15}
                </ul>
                <br/>
                <p>Puede ver el listado de pases pendientes de aprobar haciendo clic <a href='{16}'>aquí</a></p>
                <p>Usuario: {17} - {18} - id = {19}. ​</p>
            ", instancia.Jugadores.apellido + " " + instancia.Jugadores.nombre
             , instancia.Jugadores.DNI
             , instancia.Jugadores.sexo
             , instancia.Jugadores.fechaNacimiento.ToShortDateString()
             , instancia.Jugadores.Categorias != null ? instancia.Jugadores.Categorias.descripcion : ""
             , instancia.Jugadores.direccion
             , instancia.Jugadores.Localidades != null ? instancia.Jugadores.Localidades.descripcion + (instancia.Jugadores.Localidades.Provincias != null ? " - " + instancia.Jugadores.Localidades.Provincias.descripcion : "") : instancia.Jugadores.localidad
             , instancia.Jugadores.telefonoFijo
             , instancia.Jugadores.celular
             , instancia.Jugadores.mail
             , instancia.Clubes.descripcion
             , instancia.Asociaciones.descripcion
             , instancia.Federaciones.descripcion
             , instancia.Clubes1.descripcion
             , instancia.Asociaciones1.descripcion
             , instancia.Federaciones1.descripcion
             , ConfigurationManager.AppSettings["linkAplicacion"].ToString() + "/admin"
             , instancia.tipoPase
             , usuario.nombreUsuario
             , usuario.nombre
             , usuario.id);

            string asunto = "FUB - Nuevo Pase para Aprobar";
            if (instancia.tipoPase == "INTER-FEDERACION")
            {
                if (roles.Contains("Asociación"))
                {
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("confederacion"));
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("federacion", 0, instancia.idFederacionOrigen));
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("asociacion", 0, instancia.idAsociacionOrigen));
                }
                if (roles.Contains("Federacion"))
                {
                    enviarMail(asunto, body, "pases@argentinabochas.com.ar");
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("confederacion"));
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("federacion", 0, instancia.idFederacionOrigen));
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("asociacion", 0, instancia.idAsociacionOrigen));
                }
                if (roles.Contains("Confederación"))
                {
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("federacion", 0, instancia.idFederacionOrigen));
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("asociacion", 0, instancia.idAsociacionOrigen));
                }

            }

            else if (instancia.tipoPase == "INTER-ASOCIACION")
            {
                if (roles.Contains("Asociación"))
                {
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("federacion", 0, instancia.idFederacionOrigen));
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("asociacion", 0, instancia.idAsociacionOrigen));
                }
                if (roles.Contains("Federacion"))
                {
                    enviarMail(asunto, body, "pases@argentinabochas.com.ar");
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("asociacion", 0, instancia.idAsociacionOrigen));
                }
                if (roles.Contains("Confederación"))
                {
                    enviarMail(asunto, body, "pases@argentinabochas.com.ar");
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("federacion", 0, instancia.idFederacionOrigen));
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("asociacion", 0, instancia.idAsociacionOrigen));
                }

                //enviarMail(asunto, body, "pases@argentinabochas.com.ar");
                //enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("federacion", 0, instancia.idFederacionOrigen));
                
            }
            else if (instancia.tipoPase == "INTER-CLUB")
            {
                if (roles.Contains("Asociación"))
                {
                    enviarMail(asunto, body, "pases@argentinabochas.com.ar");
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("federacion", 0, instancia.idFederacionOrigen));
                }
                if (roles.Contains("Federacion"))
                {
                   
                    enviarMail(asunto, body, "pases@argentinabochas.com.ar");
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("asociacion", 0, instancia.idAsociacionOrigen));
                }
                if (roles.Contains("Confederación"))
                {
                    enviarMail(asunto, body, "pases@argentinabochas.com.ar");
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("federacion", 0, instancia.idFederacionOrigen));
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("asociacion", 0, instancia.idAsociacionOrigen));
                }

                //enviarMail(asunto, body, "pases@argentinabochas.com.ar");
                
                //enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("asociacion", 0, instancia.idAsociacionOrigen));
            }
          
        }

        /// <summary>
        /// Se envía un mail a: 
        /// - Todos los usuarios con Rol Confederación
        /// - Todos los usuarios con Rol Federación asociados a la federación de la asociación del club del nuevo jugador
        /// - Todos los usuarios con Rol Asociación asociados a la asociación del club excepto el usuario que creó la Asociación
        /// </summary>
        public static void notificacionPaseAprobado(int id, int idUsuario)
        {
            Negocio.JugadoresPases instancia = Negocio.JugadoresPasesBL.obtener(id);
            Negocio.Usuarios usuario = Negocio.UsuariosBL.obtener(idUsuario);
            var roles = Negocio.UsuariosBL.obtenerRol(idUsuario);

            //datos del pase
            string body = String.Format(@"
                <p>Se le notifica que ha sido <strong>APROBADO</strong> un nuevo Pase:</p>
                <p><strong>Datos del Jugador</strong></p>
                <ul>
                    <li><strong>Nombre: </strong>{0}</li>
                    <li><strong>DNI: </strong>{1}</li>
                    <li><strong>Sexo: </strong>{2}</li>
                    <li><strong>Fecha de Nacimiento: </strong>{3}</li>
                    <li><strong>Categoría: </strong>{4}</li>
                    <li><strong>Dirección: </strong>{5}</li>
                    <li><strong>Localidad: </strong>{6}</li>
                    <li><strong>Teléfono Fijo: </strong>{7}</li>
                    <li><strong>Celular: </strong>{8}</li>
                    <li><strong>E-Mail: </strong>{9}</li>
                </ul>
                <p><strong>Datos del Pase</strong></p>
                <ul>
                    <li><strong>Tipo de Pase: </strong>{16}
                    <li><strong>Club Origen: </strong>{10}
                    <li><strong>Asociación Origen: </strong>{11}
                    <li><strong>Federación Origen: </strong>{12}
                    <li><strong>Club Destino: </strong>{13}
                    <li><strong>Asociación Destino: </strong>{14}
                    <li><strong>Federación Destino: </strong>{15}
                </ul>
                <p>Usuario: {16} - {17} - id = {18}. ​</p>
            ", instancia.Jugadores.apellido + " " + instancia.Jugadores.nombre
             , instancia.Jugadores.DNI
             , instancia.Jugadores.sexo
             , instancia.Jugadores.fechaNacimiento.ToShortDateString()
             , instancia.Jugadores.Categorias != null ? instancia.Jugadores.Categorias.descripcion : ""
             , instancia.Jugadores.direccion
             , instancia.Jugadores.Localidades != null ? instancia.Jugadores.Localidades.descripcion + (instancia.Jugadores.Localidades.Provincias != null ? " - " + instancia.Jugadores.Localidades.Provincias.descripcion : "") : instancia.Jugadores.localidad
             , instancia.Jugadores.telefonoFijo
             , instancia.Jugadores.celular
             , instancia.Jugadores.mail
             , instancia.Clubes.descripcion
             , instancia.Asociaciones.descripcion
             , instancia.Federaciones.descripcion
             , instancia.Clubes1.descripcion
             , instancia.Asociaciones1.descripcion
             , instancia.Federaciones1.descripcion
             , instancia.tipoPase
             , usuario.nombreUsuario
             , usuario.nombre
             , usuario.id);

            string asunto = "FUB - Nuevo Pase " + instancia.tipoPase;
            //enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("confederacion"));
            //enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("federacion", idUsuario, instancia.idFederacionOrigen));
            //enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("asociacion", idUsuario, instancia.idAsociacionOrigen));

            //si es inter-federación se le notifica también a la federación destino
            if (instancia.tipoPase == "INTER-FEDERACION")
                if (roles.Contains("Confederación"))
                {
                    enviarMail(asunto, body, "pases@argentinabochas.com.ar");
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("federacion", idUsuario, instancia.idFederacionDestino));
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("asociacion", idUsuario, instancia.idAsociacionOrigen));
                }
                               

            //si es inter-asociacion se le notifica también a la asociacion destino
            if (instancia.tipoPase == "INTER-ASOCIACION")
                if (roles.Contains("Federacion"))
                {
                    
                    enviarMail(asunto, body, "pases@argentinabochas.com.ar");
                    enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("asociacion", idUsuario, instancia.idAsociacionOrigen));
                }

            //se le notifica al club origen y al club destino
            enviarMail(asunto, body, instancia.Clubes.mail);
            enviarMail(asunto, body, instancia.Clubes1.mail);
            //enviarMail(asunto, body, "pases@argentinabochas.com.ar");
        

        }

        /// <summary>
        /// Se envía un mail a: 
        /// - Todos los usuarios con Rol Confederación
        /// - Todos los usuarios con Rol Federación asociados a la federación de la asociación del club del nuevo jugador
        /// - Todos los usuarios con Rol Asociación asociados a la asociación del club excepto el usuario que creó la Asociación
        /// </summary>
        public static void notificacionPaseRechazado(int id, int idUsuario, string motivoRechazo)
        {
            Negocio.JugadoresPases instancia = Negocio.JugadoresPasesBL.obtener(id);
            Negocio.Usuarios usuario = Negocio.UsuariosBL.obtener(idUsuario);

            //datos del pase
            string body = String.Format(@"
                <p>Se le notifica que ha sido <strong>RECHAZADO</strong> el siguiente Pase:</p>
                <p><strong>Datos del Jugador</strong></p>
                <ul>
                    <li><strong>Nombre: </strong>{0}</li>
                    <li><strong>DNI: </strong>{1}</li>
                    <li><strong>Sexo: </strong>{2}</li>
                    <li><strong>Fecha de Nacimiento: </strong>{3}</li>
                    <li><strong>Categoría: </strong>{4}</li>
                    <li><strong>Dirección: </strong>{5}</li>
                    <li><strong>Localidad: </strong>{6}</li>
                    <li><strong>Teléfono Fijo: </strong>{7}</li>
                    <li><strong>Celular: </strong>{8}</li>
                    <li><strong>E-Mail: </strong>{9}</li>
                </ul>
                <p><strong>Datos del Pase</strong></p>
                <ul>
                    <li><strong>Tipo de Pase: </strong>{16}
                    <li><strong>Club Origen: </strong>{10}
                    <li><strong>Asociación Origen: </strong>{11}
                    <li><strong>Federación Origen: </strong>{12}
                    <li><strong>Club Destino: </strong>{13}
                    <li><strong>Asociación Destino: </strong>{14}
                    <li><strong>Federación Destino: </strong>{15}
                    <li><strong>Motivo del Rechzado: </strong><span style='color:red'>{17}</span>
                </ul>
                <p>Usuario: {18} - {19} - id = {20}. ​</p>
            ", instancia.Jugadores.apellido + " " + instancia.Jugadores.nombre
             , instancia.Jugadores.DNI
             , instancia.Jugadores.sexo
             , instancia.Jugadores.fechaNacimiento.ToShortDateString()
             , instancia.Jugadores.Categorias != null ? instancia.Jugadores.Categorias.descripcion : ""
             , instancia.Jugadores.direccion
             , instancia.Jugadores.Localidades != null ? instancia.Jugadores.Localidades.descripcion + (instancia.Jugadores.Localidades.Provincias != null ? " - " + instancia.Jugadores.Localidades.Provincias.descripcion : "") : instancia.Jugadores.localidad
             , instancia.Jugadores.telefonoFijo
             , instancia.Jugadores.celular
             , instancia.Jugadores.mail
             , instancia.Clubes.descripcion
             , instancia.Asociaciones.descripcion
             , instancia.Federaciones.descripcion
             , instancia.Clubes1.descripcion
             , instancia.Asociaciones1.descripcion
             , instancia.Federaciones1.descripcion
             , instancia.tipoPase
             , motivoRechazo
             , usuario.nombreUsuario
             , usuario.nombre
             , usuario.id);

            string asunto = "FUB - Se ha rechazado un Pase " + instancia.tipoPase;
            enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("confederacion"));
            enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("federacion", idUsuario, instancia.idFederacionOrigen));
            enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("asociacion", idUsuario, instancia.idAsociacionOrigen));

            //si es inter-federación se le notifica también a la federación destino
            if (instancia.tipoPase == "INTER-FEDERACION")
                enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("federacion", idUsuario, instancia.idFederacionDestino));

            //si es inter-asociacion se le notifica también a la asociacion destino
            if (instancia.tipoPase == "INTER-ASOCIACION")
                enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("asociacion", idUsuario, instancia.idAsociacionDestino));

            //se le notifica al club origen y al club destino
            enviarMail(asunto, body, instancia.Clubes.mail);
            enviarMail(asunto, body, instancia.Clubes1.mail);
        }

        public static void notificacionNuevaPenalizacion(int id, bool requiereAprobacionFederacion)
        {
            Negocio.JugadoresPenalizaciones instancia = Negocio.JugadoresPenalizacionesBL.obtener(id);

            //datos del pase
            string body = String.Format(@"
                <p>Se le notifica que ha creado la siguiente Penalizacion:</p>
                <p><strong>Datos del Jugador</strong></p>
                <ul>
                    <li><strong>Nombre: </strong>{0}</li>
                    <li><strong>DNI: </strong>{1}</li>
                    <li><strong>Sexo: </strong>{2}</li>
                    <li><strong>Fecha de Nacimiento: </strong>{3}</li>
                    <li><strong>Categoría: </strong>{4}</li>
                    <li><strong>Dirección: </strong>{5}</li>
                    <li><strong>Localidad: </strong>{6}</li>
                    <li><strong>Teléfono Fijo: </strong>{7}</li>
                    <li><strong>Celular: </strong>{8}</li>
                    <li><strong>E-Mail: </strong>{9}</li>
                    <li><strong>Club: </strong>{10}</li>
                    <li><strong>Asociación: </strong>{11}</li>
                    <li><strong>Federación: </strong>{12}</li>
                </ul>
                <p><strong>Datos de la Penalización</strong></p>
                <ul>
                    <li><strong>Fecha de Inicio: </strong>{13}
                    <li><strong>Días de Penalización: </strong>{14}
                    <li><strong>Fecha de Finalización: </strong>{15}
                    <li><strong>Motivos: </strong>{16}
                    <li><strong>Creada por: </strong>{17}
                </ul>
            ", instancia.Jugadores.apellido + " " + instancia.Jugadores.nombre
             , instancia.Jugadores.DNI
             , instancia.Jugadores.sexo
             , instancia.Jugadores.fechaNacimiento.ToShortDateString()
             , instancia.Jugadores.Categorias != null ? instancia.Jugadores.Categorias.descripcion : ""
             , instancia.Jugadores.direccion
             , instancia.Jugadores.Localidades != null ? instancia.Jugadores.Localidades.descripcion + (instancia.Jugadores.Localidades.Provincias != null ? " - " + instancia.Jugadores.Localidades.Provincias.descripcion : "") : instancia.Jugadores.localidad
             , instancia.Jugadores.telefonoFijo
             , instancia.Jugadores.celular
             , instancia.Jugadores.mail
             , instancia.Jugadores.Clubes.descripcion
             , instancia.Jugadores.Clubes.Asociaciones.descripcion
             , instancia.Jugadores.Clubes.Asociaciones.Federaciones.descripcion
             , instancia.fechaInicio.ToShortDateString()
             , instancia.dias.ToString()
             , instancia.fechaFin.ToShortDateString()
             , instancia.motivos
             , instancia.Usuarios.nombre);

            string asunto = "FUB - Se ha creado una nueva penalización";

            //se envía mail a todos los usuarios con rol Confederación
            enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("confederacion"));

            //se envía un mail a todos los usuarios con rol Federación
            if (requiereAprobacionFederacion)
                enviarMail(asunto, body, Negocio.UsuariosBL.obtenerUsuariosPorRol("federacion", 0, instancia.Jugadores.Clubes.Asociaciones.idFederacion));
        }

        private static void enviarMail(string subject, string body, List<Negocio.Usuarios> mailsTo)
        {
            if (mailsTo != null && mailsTo.Count > 0)
            {
                MailMessage mensaje = new MailMessage();
                foreach (var mail in mailsTo)
                {
                    try
                    {
                        mensaje.To.Add(new MailAddress(mail.mail));
                    }
                    catch
                    {
                    }
                }

                if (mensaje.To.Count > 0)
                {
                    mensaje.From = new MailAddress(ConfigurationManager.AppSettings["RecuperarPasswordFrom"].ToString());
                    mensaje.Subject = subject;
                    mensaje.IsBodyHtml = true;
                    mensaje.Body = body;
                    //Integrar.Mail.Mail.EnviarMail(mensaje);
                }
            }
        }

        private static void enviarMail(string subject, string body, string mailTo)
        {
            if (mailTo != null && mailTo.Length > 0)
            {
                MailMessage mensaje = new MailMessage();
                try
                {
                    mensaje.To.Add(new MailAddress(mailTo));
                }
                catch
                {
                }

                if (mensaje.To.Count > 0)
                {
                   
                    mensaje.From = new MailAddress(ConfigurationManager.AppSettings["RecuperarPasswordFrom"].ToString());
                    mensaje.Subject = subject;
                    mensaje.IsBodyHtml = true;
                    mensaje.Body = body;
                    Integrar.Mail.Mail.EnviarMail(mensaje);
                }
            }
        }
    }
}
