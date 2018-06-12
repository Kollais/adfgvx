using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        int mode = 0; //режим работы, 0 - шифровать, 1 - расшифровыать
        char[,] table = new char[6, 6]; //таблица с алфавитом adfgvx
        struct li //структура символ-индекс
        {
            public char let;
            public int ind;
        };

        public Form1()
        {
            InitializeComponent();
            //инициализация данных для визуальной формы
            this.Text = "ADFGVX";
            label1.Text = "Режим";
            label2.Text = "Изначальный текст";
            label3.Text = "Зашифрованный текст";
            label4.Text = "Секретное слово";
            button1.Text = "применить";
            comboBox1.Items.Add("зашифровывать");
            comboBox1.Items.Add("расшифровывать");
            comboBox1.SelectedIndex = 0;
            //инициализация таблицы-алфавита
            dataGridView1.Columns.Add("","");
            dataGridView1.Columns.Add("A","A");
            dataGridView1.Columns.Add("D", "D");
            dataGridView1.Columns.Add("F", "F");
            dataGridView1.Columns.Add("G", "G");
            dataGridView1.Columns.Add("V", "V");
            dataGridView1.Columns.Add("X", "X");
            dataGridView1.Columns[0].Width = 20;
            dataGridView1.Columns[1].Width = 20;
            dataGridView1.Columns[2].Width = 20;
            dataGridView1.Columns[3].Width = 20;
            dataGridView1.Columns[4].Width = 20;
            dataGridView1.Columns[5].Width = 20;
            dataGridView1.Columns[6].Width = 20;
            dataGridView1.Rows.Add("A");
            dataGridView1.Rows.Add("D");
            dataGridView1.Rows.Add("F");
            dataGridView1.Rows.Add("G");
            dataGridView1.Rows.Add("V");
            dataGridView1.Rows.Add("X");
            //генерация и вывод на экран таблицы-алфавита
            table = fill_table_rand();
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    dataGridView1.Rows[i].Cells[j + 1].Value = table[i, j];
                }
            }
        }

        char[,] fill_table_rand() //заполнение таблицы фиксированным алфавитом в случайном порядке
        {
            string alb = "abcdefghijklmnopqrstuvwxyz0123456789"; //фиксированный алфавит
            Random rnd = new Random();
            int nxt;
            char[,] tbl = new char[6, 6];

            for (int i = 0; i < 6; i++) //для каждого символа в двумерном массиве
            {
                for (int j = 0; j < 6; j++)
                {
                    nxt = rnd.Next(alb.Length); //генеруем случайное число в диапазоне размера строки алфавита
                    tbl[i, j] = alb[nxt]; //записываем букву с соответсвующим индексом в таблицу
                    alb = alb.Remove(nxt, 1); //удаляем использованную букву
                }
            }
            return tbl;
        }

        static char[,] table_swap_cols(char[,] tbl, int n1, int n2, int len) //меняем столбцы с индексами n1  и n2 местами
        {
            char buf;
            for (int i = 0; i < len; i++) //len - длина кодового слова
            {
                buf = tbl[i, n1];
                tbl[i, n1] = tbl[i, n2];
                tbl[i, n2] = buf;
            }
            return tbl;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //меняем режим работы при выборе 
                                                                                //варианта из выпадающего списка
        {
            if (comboBox1.SelectedIndex == 0)
                mode = 0; //шифровать
            else
                mode = 1; //расшифровывать
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string a = "ADFGVX";
            string mes, key, sh="";
            key = textBox3.Text; //получаем кодовое слово (ключ) из текстового поля
            key = key.Replace(" ", string.Empty); //удаляем из него всего пробелы
            key = key.ToUpper(); //переводим в верхний регистр
            if (mode == 0) //если нужно зашифровывать
            {
                mes = textBox1.Text; //получаем текст оригинального сообщения
                mes = mes.Replace(" ", string.Empty); //удаляем из него пробелы
                mes = mes.ToLower(); //переводим в нижний регистр

                if (mes.Length>key.Length*key.Length) //если сообщение не влезает в заданный квадрат, ничего не делаем
                {
                    MessageBox.Show("Слишком длинное сообщение");
                }
                else //иначе, сам алгоритм шифрования
                {
                    string cm = "";
                    int in1 = 0, in2 = 0, fl = 0;
                    for (int i = 0; i < mes.Length; i++) //делаем замену каждой буквы на ее буквенные координа в таблице-алфавите
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            for (int k = 0; k < 6; k++)
                            {
                                if (mes[i] == table[j, k])
                                {
                                    in1 = j; in2 = k; fl = 1;
                                    break;
                                }
                            }
                            if (fl == 1)
                            {
                                fl = 0; break;
                            }
                        }
                        cm += a[in2]; 
                        cm += a[in1]; //и записываем их в отдельную строку
                    }

                    char[,] tb1 = new char[key.Length, key.Length];
                    int ind = 0;
                    for (int i = 0; i < key.Length; i++) //переводим эту строку в двумерный массив строк
                    {
                        for (int j = 0; j < key.Length; j++)
                        {
                            if (ind < cm.Length)
                            {
                                tb1[i, j] = cm[ind];
                                ind++;
                            }
                            else
                                tb1[i, j] = ' '; //если сообщение короче максимума, дозаполняем пробелами
                        }
                    }

                    char[] key_n = new char[key.Length];
                    key_n = key.ToCharArray(); //переводим ключ из строки в массив символов
                    for (int i = 0; i < key.Length -1 ; i++) //сортируем ключ в алфавитном порядке вместе с подчиненными каждой
                                                             //букве столбцами таблицы с зашифрованным сообщением
                    {
                        for (int j = 0; j < key.Length - 1; j++)
                        {
                            if (key_n[j] > key_n[j + 1])
                            {
                                char buf = key_n[j];
                                key_n[j] = key_n[j + 1];
                                key_n[j + 1] = buf;
                                table_swap_cols(tb1, j, j + 1,key.Length);
                            }
                        }
                    }

                    for (int i = 0; i < key.Length; i++) //переводим получившееся в строку, читая по столбцам
                    {
                        for (int j = 0; j < key.Length; j++)
                        {
                            sh += tb1[j, i];
                        }
                        sh += " ";
                    }
                    textBox2.Text = sh; //выводим в ткстовое поле
                }
            }
            else //если расшифровываем
            {
                mes = textBox2.Text; //получаем зашифрованный текст из текстового поля
                mes = mes.ToLower(); //переводим все в нижний регистр
                string cm = "";
                int ind = 1;
                for (int i = 0; i < mes.Length; i++) //записываем в строку, пропуская незначащие пробелы
                {
                    if (ind % (key.Length+1) != 0)
                        cm += mes[i];
                    ind++;
                }

                if (cm.Length > key.Length * key.Length) //если сообщение длинное, ничего не делаем
                {
                    MessageBox.Show("Слишком длинное сообщение");
                }
                else //если все нормально - дешифруем
                {
                    ind = 0;
                    char[,] tb1 = new char[key.Length, key.Length];
                    for (int i = 0; i < key.Length; i++) //переводим строку в двумерный массив символов
                    {
                        for (int j = 0; j < key.Length; j++)
                        {
                            tb1[j, i] = cm[ind];
                            ind++;
                        }
                    }

                    int[] ind_k = new int[key.Length];
                    for (int i = 0; i < key.Length; i++) //делаем массив номеров от 0 до длины ключа
                    {
                        ind_k[i] = i;
                    }

                    char[] key_n = new char[key.Length];
                    key_n = key.ToCharArray(); //переводим ключ в массив символов

                    for (int i = 0; i < key.Length - 1; i++) //сортируем ключ в алфавитном порядке, одновременно с массивом индексов
                                                             //получаем запутанный массив индексов
                    {
                        for (int j = 0; j < key.Length - 1; j++)
                        {
                            if (key_n[j] > key_n[j + 1])
                            {
                                char buf = key_n[j];
                                key_n[j] = key_n[j + 1];
                                key_n[j + 1] = buf;
                                int ib = ind_k[j];
                                ind_k[j] = ind_k[j + 1];
                                ind_k[j + 1] = ib;
                            }
                        }
                    }

                    for (int i = 0; i < key.Length - 1; i++) //сортируем массив индексов обратно в порядок
                                                             //а вместе с ним массив символов ключа
                                                             //и колонки двумерного массива с зашифрованным текстом
                    {
                        for (int j = 0; j < key.Length - 1; j++)
                        {
                            if (ind_k[j] > ind_k[j + 1])
                            {
                                int ib = ind_k[j];
                                ind_k[j] = ind_k[j + 1];
                                ind_k[j + 1] = ib;
                                char buf = key_n[j];
                                key_n[j] = key_n[j + 1];
                                key_n[j + 1] = buf;
                                table_swap_cols(tb1, j, j + 1, key.Length);
                            }
                        }
                    }

                    string ct = "";
                    for (int i = 0; i < key.Length; i++) //записываем получившееся в строку
                    {
                        for (int j = 0; j < key.Length; j++)
                        {
                            ct += tb1[i, j];
                        }
                    }
                    ct = ct.Replace(" ", string.Empty); //убираем пробелы в конце (если есть)


                    li[] st = new li[6]; //делаем массив структурок, где каждой букве шифра adfgvx ставим в соответсвие ее индекс
                    st[0].ind = 0;
                    st[0].let = 'a';
                    st[1].ind = 1;
                    st[1].let = 'd';
                    st[2].ind = 2;
                    st[2].let = 'f';
                    st[3].ind = 3;
                    st[3].let = 'g';
                    st[4].ind = 4;
                    st[4].let = 'v';
                    st[5].ind = 5;
                    st[5].let = 'x';

                    string dt = "";
                    int ind1=0, ind2=0;
                    for(int i=0;i<ct.Length;i++) //каждую пару символов переводим в координаты с помошью структуры выше
                                                 //и с их помощью находим изначальную зашифрованную букву
                    {
                        for (int j=0;j<6;j++)
                        {
                            if(ct[i]==st[j].let)
                            {
                                ind1 = st[j].ind;
                                break;
                            }
                        }
                        i++;
                        for (int j = 0; j < 6; j++)
                        {
                            if (ct[i] == st[j].let)
                            {
                                ind2 = st[j].ind;
                                break;
                            }
                        }
                        dt += table[ind2,ind1]; //дописываем ее в строку
                    }

                    //получаем расшифрованный текст (без пробелов, их восстановить невозможно)
                    textBox1.Text = dt; //записываем его в соответствующее текстовое поле
                }
            }
            

        }

        
    }
}
