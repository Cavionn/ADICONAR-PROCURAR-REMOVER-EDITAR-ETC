using System;
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
        private string data_source = "datasource=localhost;port=3306;username=root;password='';database=db_agenda2";
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

                // Comando SQL para inserção
                string sql = "INSERT INTO coo (nome, email, telefone) VALUES (@nome, @email, @telefone)"; //inserir um novo registro na tabela
                MySqlCommand comando = new MySqlCommand(sql, Conecao); //cria um objeto MySqlCommand que representa um comando SQL a ser executado

                comando.Connection = Conecao; //definir a conexão que um comando deve usar para se comunicar com um banco de dados.

                // Usando parâmetros
                
                comando.Prepare();

                comando.Parameters.AddWithValue("@nome", txtNome.Text);
                comando.Parameters.AddWithValue("@email", txtEmail.Text);
                comando.Parameters.AddWithValue("@telefone", txtTelefone.Text);

                // Abrir conexão
                Conecao.Open();

                // Executar comando
                comando.ExecuteNonQuery();

                MessageBox.Show("Dados inseridos com sucesso!");
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
    }
}
