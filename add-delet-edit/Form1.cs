﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace add_delet_edit
{
    public partial class Form1 : Form
    {
        private MySqlConnection Conecao;

        private string data_source = "datasource=localhost;port=3306;username=root;password='';database=db_Agenda2";

        private int? id_contato_sel = null;
        public Form1()
        {
            InitializeComponent();

            lsvLista.View = View.Details;
            lsvLista.LabelEdit = true;
            lsvLista.AllowColumnReorder = true;
            lsvLista.GridLines = true;

            lsvLista.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lsvLista.Columns.Add("Nome", 150, HorizontalAlignment.Left);
            lsvLista.Columns.Add("Email", 150, HorizontalAlignment.Left);
            lsvLista.Columns.Add("Telefone", 150, HorizontalAlignment.Left);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                // Criar conexão MySQL
                Conecao = new MySqlConnection(data_source); //criar uma nova instância da classe

                MySqlCommand cmd = new MySqlCommand();

                // Comando SQL para inserção
                cmd.Connection = Conecao;

                Conecao.Open();
             
                cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                cmd.Parameters.AddWithValue("@id", id_contato_sel);

             
                if(id_contato_sel == null) //Caso a caixa for nula fazer um insert -- nula diferente de zero
                {
                    cmd.CommandText = "INSERT INTO coo (nome, email, telefone) VALUES (@nome, @email, @telefone)"; //inserir um novo registro na tabela

                    cmd.Prepare();

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.CommandText = "UPDATE coo SET nome=@nome, email=@email, telefone=@telefone WHERE id=@id"; //inserir um novo registro na tabela

                    cmd.Prepare();

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("O contato foi atualizado",
                        "Concluido com Sucesso" , MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                

                /*MySqlCommand comando = new MySqlCommand(cmd.CommandText, Conecao);*/ //cria um objeto MySqlCommand que representa um comando SQL a ser executado


                cmd.Connection = Conecao; //definir a conexão que um comando deve usar para se comunicar com um banco de dados.


                // Executar comando
                cmd.ExecuteNonQuery();

                MessageBox.Show("Dados inseridos com sucesso!");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show("Erro: " + ex.Number + "Ocorreu: " + ex.Message , 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Exibe a mensagem de erro
            }
            finally
            {
                Conecao.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string q = "%" + txtBusca.Text + "%"; // Adiciona os % para a busca

                // Criar conexão MySQL
                Conecao = new MySqlConnection(data_source);

                // Comando SQL para buscar os dados
                string sql = "SELECT * FROM coo WHERE nome LIKE @q OR email LIKE @q OR id LIKE @q";
                MySqlCommand comando = new MySqlCommand(sql, Conecao);

                // Usando parâmetros 
                comando.Parameters.AddWithValue("@q", q);

                // Abrir conexão
                Conecao.Open();

                // Executar comando
                MySqlDataReader reader = comando.ExecuteReader();

                // Limpar itens do ListView antes de adicionar novos
                lsvLista.Items.Clear();

                // Preencher o ListView com os resultados
                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetValue(0).ToString(),// ID
                        reader.GetString(1),// Nome
                        reader.GetString(2),//Email
                        reader.GetValue(3).ToString()// Telefone
                    };

                    var linha_listview = new ListViewItem(row);
                    lsvLista.Items.Add(linha_listview);
                }

                MessageBox.Show("Busca realizada com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message); // Exibe a mensagem de erro
            }
            finally
            {
                Conecao.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string q = "%" + txtDeletar.Text + "%"; // Adiciona os % para a busca

                // Criar conexão MySQL
                Conecao = new MySqlConnection(data_source);

                // Comando SQL para buscar os dados
                string sql = "DELETE FROM coo WHERE id LIKE @q";
                MySqlCommand comando = new MySqlCommand(sql, Conecao);

                // Usando parâmetros para evitar SQL Injection
                comando.Parameters.AddWithValue("@q", q);

                // Abrir conexão
                Conecao.Open();

                // Executar comando
                MySqlDataReader reader = comando.ExecuteReader();

                // Limpar itens do ListView antes de adicionar novos
                lsvLista.Items.Clear();

                // Preencher o ListView com os resultados
                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetValue(0).ToString(),// ID
                        reader.GetString(1),// Nome
                        reader.GetString(2),//Email
                        reader.GetValue(3).ToString()// Telefone
                };

                    var linha_listview = new ListViewItem(row);
                    lsvLista.Items.Add(linha_listview);
                }

                MessageBox.Show("Busca realizada com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message); // Exibe a mensagem de erro
            }
            finally
            {
                Conecao.Close();
            }
        }

        private void lsvLista_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items_selecionados = lsvLista.SelectedItems; //Salvou a alteração dentro do item selecionado e escolhe qual pegar 

            foreach (ListViewItem item in items_selecionados)//Criação de uma classe 
            {
                id_contato_sel = Convert.ToInt32(item.SubItems[0].Text);

                txtNome.Text = item.SubItems[1].Text;//Percorrendo a lista da lista, e pede para ler o text box 1 e pegar o sub item do item 1 e jogar no campo

                txtEmail.Text = item.SubItems[2].Text;

                txtTelefone.Text = item.SubItems[3].Text;

            } 

        }
    }
}
