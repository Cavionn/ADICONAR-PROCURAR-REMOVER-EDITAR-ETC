using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
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

        private string data_source = "datasource=localhost;port=3306;username=root;password='';database=db_agenda";

        private int? id_contatos_selecionado = null;
        public Form1()
        {
            InitializeComponent();


            lsvLista.View = View.Details;
            lsvLista.LabelEdit = true;
            lsvLista.AllowColumnReorder = true;
            lsvLista.GridLines = true;

            //Organização do List View
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

                Conecao.Open(); //Abrea a conecao com o banco de dados

                //Passa o parametro cmd e adiciona o valor, adicionando parâmetros a um comando SQL

                cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                cmd.Parameters.AddWithValue("@id", id_contatos_selecionado);

             
                if(id_contatos_selecionado == null) //Caso a caixa for nula fazer um insert -- nula diferente de zero
                {
                    cmd.CommandText = "INSERT INTO contato (nome, email, telefone) VALUES (@nome, @email, @telefone)"; //inserir um novo registro na tabela


                    cmd.Prepare();

                    cmd.ExecuteNonQuery(); // para executar uma instrução SQL que não retorna linhas, como INSERT, UPDATE, DELETE, ou operações de catálogo (como criar tabelas). Esse método retorna o número de linhas afetadas pela execução do comando
                }
                else
                {
                    cmd.CommandText = "UPDATE contato SET nome=@nome, email=@email, telefone=@telefone WHERE id=@id"; //inserir um novo registro na tabela

                    cmd.Prepare(); //preparar um comando SQL antes de sua execução.

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
                Conecao.Close(); //fecha a conecao com o banco de dados
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
                string sql = "SELECT * FROM contato WHERE nome LIKE @q OR email LIKE @q OR id LIKE @q";
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
                DialogResult conf = MessageBox.Show("Você deseja fazer a exclusão ?", "Você realmente tem certeza ?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (conf == DialogResult.Yes)
                {


                    MySqlCommand comando = new MySqlCommand();

                    Conecao = new MySqlConnection(data_source);

                    Conecao.Open();//Abre a Conexão

                    if (lsvLista.SelectedItems.Count > 0)
                    {


                        string identificador = lsvLista.SelectedItems[0].Text; //atribuindo o texto do primeiro item selecionado em um controle ListView a uma variável
                        comando.Connection = Conecao;



                        comando.CommandText = "DELETE FROM contato WHERE id=" + identificador;

                        comando.Prepare();

                        comando.ExecuteNonQuery();

                        MessageBox.Show("O contato foi removido",
                                           "Concluido com Sucesso",
                                           MessageBoxButtons.OK,
                                           MessageBoxIcon.Information);

                        MySqlDataReader reader = comando.ExecuteReader();
                    }
                }
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
                id_contatos_selecionado = Convert.ToInt32(item.SubItems[0].Text); //Converte a variavel global id_contatos_selecionados

                //Percorrendo a lista da lista, e pede para ler o text box 1 e pegar o sub item do item 1 e jogar no campo

                txtNome.Text = item.SubItems[1].Text;

                txtEmail.Text = item.SubItems[2].Text;

                txtTelefone.Text = item.SubItems[3].Text;

            } 

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //Variavel conf armasena a resposta do usuario dentro da avriavel para ser checada no if
                DialogResult conf = MessageBox.Show("Você deseja fazer a exclusão ?", "Você realmente tem certeza ?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); //Exibir uma mensagem de erro antes da exclusão do item
                
                if (conf == DialogResult.Yes)
                {

                    //Criar uma conexão
                    Conecao = new MySqlConnection(data_source);
                    Conecao.Open();
                    //Cria a variavel comando
                    MySqlCommand comando = new MySqlCommand();

                    // Comando SQL para inserção
                    comando.Connection = Conecao;

                    comando.Parameters.AddWithValue("@id", id_contatos_selecionado);//Usa a propriedade do parametro 
                  
                    comando.CommandText = "DELETE FROM contato WHERE id=@id"; //Deletar um item

                    comando.Prepare(); //preparar um comando SQL antes de sua execução.

                    comando.ExecuteNonQuery();

                    MessageBox.Show("O contato foi removido",
                                    "Concluido com Sucesso", 
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show("Erro: " + ex.Number + " Ocorreu " + ex.Number + " Erro");
            }

        }

       
    }
}
