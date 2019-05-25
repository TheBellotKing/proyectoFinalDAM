﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsDAMapp.Helpers;
using WindowsFormsDAMapp.Entidades;
using RestSharp;
using Newtonsoft.Json;


namespace WindowsFormsDAMapp
{
    public partial class formClasificacion : Form
    {
        sessionInfo infoSesion;
        RestClient restClient = new RestClient();
        webServiceInfo session = new webServiceInfo();
        List<potenciaJugadorEntity> listaClasificacion;

        public formClasificacion(sessionInfo infoSesion)
        {
            this.infoSesion = infoSesion;
            InitializeComponent();
        }

        private void FormClasificacion_Load(object sender, EventArgs e)
        {
            // Introducimos la cadena del servicio
            restClient = new RestClient(session.CadenaConexion);

            // Obtenemos la clasificacion de la partida
            listaClasificacion = obtenerClasificacion();

            // Mostramos la clasificacion de la partida
            mostrarClasificacion(listaClasificacion);


        }

        // Obtenemos la clasificacion de la partida
        private List<potenciaJugadorEntity> obtenerClasificacion()
        {
            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Jugadores/obtenerClasificacion", Method.GET);

            // Añadimos el id de la partida a la peticion
            peticion.AddParameter("id_Partida", infoSesion.id_partida);

            // Obtenemos el resultado de la peticion
            var response = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para almacenarlo
            List<potenciaJugadorEntity> result = JsonConvert.DeserializeObject<List<potenciaJugadorEntity>>(response.Content);

            return result;
        }

        // Metodo para visualizar la clasificacione en el listView
        private void mostrarClasificacion(List<potenciaJugadorEntity> clasificacion)
        {
            lsv_clasificacion.Items.Clear();

            foreach (var jugador in clasificacion)
            {
                // Introducimos la informacion en el listview
                ListViewItem itemAux = new ListViewItem(jugador.nombreJugador);
                itemAux.SubItems.Add(obtenerNumeroPueblos(jugador.nombreJugador).ToString());
                itemAux.SubItems.Add(jugador.potenciaJugador.ToString());

                lsv_clasificacion.Items.Add(itemAux);
            }
        }

        private int obtenerNumeroPueblos(string nombreUsuario)
        {
            // Creamos un objeto para realizar la peticion el web service
            RestRequest peticion = new RestRequest("/api/Pueblo/obtenerListaPueblos", Method.GET);

            // Añadimos el nombre del usuario a la peticion
            peticion.AddParameter("propietario", nombreUsuario);

            // Obtenemos el resultado de la peticion
            var response = restClient.Execute(peticion);

            // Deserializamos el resultado de la peticion recibido para almacenarlo
            List<puebloEntity> result = JsonConvert.DeserializeObject<List<puebloEntity>>(response.Content);

            // Devolvemos el numero de elementos en la lista
            return result.Count;

        }

        private void Btn_visionGeneral_Click(object sender, EventArgs e)
        {
            // Creamos un objeto del formulario de inicio de sesion
            formVisionGeneral VisionGeneral = new formVisionGeneral(infoSesion);

            // Lanzamos el objeto de inicio de sesion   
            VisionGeneral.Show();

            // Cerramos este formulario
            this.Close();
        }

        private void Btn_reclutamiento_Click(object sender, EventArgs e)
        {
            // Creamos un objeto del formulario de reclutamiento
            formReclutamiento reclutamiento = new formReclutamiento(infoSesion);

            // Lanzamos el formulario de reclutamiento
            reclutamiento.Show();

            // Cerramos este formulario
            this.Close();
        }

        // Metodo para volver al menu principal
        private void Btn_volver_Click(object sender, EventArgs e)
        {
            frm_MenuPrincipal formularioPrincipal = new frm_MenuPrincipal();

            formularioPrincipal.Show();

            this.Close();
        }

        // Metodo asociado a la recarga de la vista
        private void Btn_Clasificacion_Click(object sender, EventArgs e)
        {
            // Obtenemos la clasificacion de la partida
            listaClasificacion = obtenerClasificacion();

            // Mostramos la clasificacion de la partida
            mostrarClasificacion(listaClasificacion);
        }
    }
}
