using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace BibliotekaApp
{
    public partial class Form1 : Form
    {
        private string connectionString =
            "Data Source=DESKTOP-Q6VHS1L;Initial Catalog=BibliotekaDB;Integrated Security=True";

        // --- AUTORI ---
        private Label lblIme, lblPrezime, lblDrzava, lblGodina;
        private TextBox txtIme, txtPrezime, txtDrzava, txtGodina;
        private Button btnDodajAutor;
        private DataGridView dataGridAutori;
        private int? selectedAuthorId = null;

        // --- KNJIGE ---
        private Label lblNaslov, lblGodinaKnj, lblISBN, lblAutorKnj, lblKolicina;
        private TextBox txtNaslov, txtGodinaKnj, txtISBN, txtKolicina;
        private ComboBox cmbAutorKnj;
        private Button btnDodajKnjigu;
        private DataGridView dataGridKnjige;
        private int? selectedBookId = null;

        // --- ČLANOVI ---
        private Label lblClanIme, lblClanPrezime, lblClanEmail, lblClanTelefon, lblClanJoin;
        private TextBox txtClanIme, txtClanPrezime, txtClanEmail, txtClanTelefon;
        private DateTimePicker dtpClanJoin;
        private Button btnDodajClana;
        private DataGridView dataGridClanovi;
        private int? selectedMemberId = null;

        // --- POSUDBE ---
        private Label lblPosClan, lblPosKnjiga, lblPosDatum, lblPosPovratak;
        private ComboBox cmbPosClan, cmbPosKnjiga;
        private DateTimePicker dtpPosDatum, dtpPosPovratak;
        private Button btnDodajPosudbu;
        private DataGridView dataGridPosudbe;
        private int? selectedLoanId = null;
        private Label lblStatusPosudbe;

        public Form1()
        {
            InitializeComponent();
            KreirajUI();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                LoadAutori();
                LoadAutoriCombo();
                LoadKnjige();
                LoadClanovi();
                LoadClanoviComboPosudbe();
                LoadKnjigeComboPosudbe();
                LoadPosudbe();

                dataGridAutori.ClearSelection();
                dataGridKnjige.ClearSelection();
                dataGridClanovi.ClearSelection();
                dataGridPosudbe.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju podataka: " + ex.Message);
            }
        }

        private void KreirajUI()
        {
            this.Text = "Biblioteka";
            this.ClientSize = new Size(800, 700);
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(0, 1600);

            // ===== AUTORI =====
            lblIme = new Label { Text = "Ime:", Location = new Point(20, 20), AutoSize = true };
            this.Controls.Add(lblIme);
            txtIme = new TextBox { Location = new Point(120, 20), Width = 150 };
            this.Controls.Add(txtIme);

            lblPrezime = new Label { Text = "Prezime:", Location = new Point(20, 60), AutoSize = true };
            this.Controls.Add(lblPrezime);
            txtPrezime = new TextBox { Location = new Point(120, 60), Width = 150 };
            this.Controls.Add(txtPrezime);

            lblDrzava = new Label { Text = "Država:", Location = new Point(20, 100), AutoSize = true };
            this.Controls.Add(lblDrzava);
            txtDrzava = new TextBox { Location = new Point(120, 100), Width = 150 };
            this.Controls.Add(txtDrzava);

            lblGodina = new Label { Text = "God. rođenja:", Location = new Point(20, 140), AutoSize = true };
            this.Controls.Add(lblGodina);
            txtGodina = new TextBox { Location = new Point(120, 140), Width = 150 };
            this.Controls.Add(txtGodina);

            btnDodajAutor = new Button
            {
                Text = "Dodaj autora",
                Location = new Point(20, 180),
                Size = new Size(250, 35)
            };
            btnDodajAutor.Click += btnDodajAutor_Click;
            this.Controls.Add(btnDodajAutor);

            var btnObrisi = new Button
            {
                Text = "Obriši autora",
                Location = new Point(20, 220),
                Size = new Size(250, 35)
            };
            btnObrisi.Click += btnObrisi_Click;
            this.Controls.Add(btnObrisi);

            var btnUredi = new Button
            {
                Text = "Spasi izmjene",
                Location = new Point(20, 260),
                Size = new Size(250, 35)
            };
            btnUredi.Click += btnUredi_Click;
            this.Controls.Add(btnUredi);

            var btnPonisti = new Button
            {
                Text = "Poništi odabir",
                Location = new Point(20, 300),
                Size = new Size(250, 35)
            };
            btnPonisti.Click += btnPonisti_Click;
            this.Controls.Add(btnPonisti);

            dataGridAutori = new DataGridView
            {
                Location = new Point(300, 20),
                Size = new Size(450, 300),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dataGridAutori.SelectionChanged += dataGridAutori_SelectionChanged;
            this.Controls.Add(dataGridAutori);

            // ===== KNJIGE =====
            lblNaslov = new Label { Text = "Naslov:", Location = new Point(20, 360), AutoSize = true };
            this.Controls.Add(lblNaslov);
            txtNaslov = new TextBox { Location = new Point(120, 360), Width = 150 };
            this.Controls.Add(txtNaslov);

            lblGodinaKnj = new Label { Text = "God. izdanja:", Location = new Point(20, 400), AutoSize = true };
            this.Controls.Add(lblGodinaKnj);
            txtGodinaKnj = new TextBox { Location = new Point(120, 400), Width = 150 };
            this.Controls.Add(txtGodinaKnj);

            lblISBN = new Label { Text = "ISBN:", Location = new Point(20, 440), AutoSize = true };
            this.Controls.Add(lblISBN);
            txtISBN = new TextBox { Location = new Point(120, 440), Width = 150 };
            this.Controls.Add(txtISBN);

            lblKolicina = new Label { Text = "Količina:", Location = new Point(20, 480), AutoSize = true };
            this.Controls.Add(lblKolicina);
            txtKolicina = new TextBox { Location = new Point(120, 480), Width = 150 };
            this.Controls.Add(txtKolicina);

            lblAutorKnj = new Label { Text = "Autor:", Location = new Point(20, 520), AutoSize = true };
            this.Controls.Add(lblAutorKnj);
            cmbAutorKnj = new ComboBox
            {
                Location = new Point(120, 520),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbAutorKnj);

            btnDodajKnjigu = new Button
            {
                Text = "Dodaj knjigu",
                Location = new Point(20, 560),
                Size = new Size(250, 35)
            };
            btnDodajKnjigu.Click += btnDodajKnjigu_Click;
            this.Controls.Add(btnDodajKnjigu);

            var btnObrisiKnj = new Button
            {
                Text = "Obriši knjigu",
                Location = new Point(20, 600),
                Size = new Size(250, 35)
            };
            btnObrisiKnj.Click += btnObrisiKnjigu_Click;
            this.Controls.Add(btnObrisiKnj);

            var btnUrediKnj = new Button
            {
                Text = "Spasi izmjene knjige",
                Location = new Point(20, 640),
                Size = new Size(250, 35)
            };
            btnUrediKnj.Click += btnUrediKnjigu_Click;
            this.Controls.Add(btnUrediKnj);

            var btnPonistiKnj = new Button
            {
                Text = "Poništi odabir knjige",
                Location = new Point(20, 680),
                Size = new Size(250, 35)
            };
            btnPonistiKnj.Click += btnPonistiKnj_Click;
            this.Controls.Add(btnPonistiKnj);

            dataGridKnjige = new DataGridView
            {
                Location = new Point(300, 360),
                Size = new Size(450, 320),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dataGridKnjige.SelectionChanged += dataGridKnjige_SelectionChanged;
            this.Controls.Add(dataGridKnjige);

            // ===== ČLANOVI =====
            int clanY = 720;

            lblClanIme = new Label { Text = "Ime člana:", Location = new Point(20, clanY), AutoSize = true };
            this.Controls.Add(lblClanIme);
            txtClanIme = new TextBox { Location = new Point(120, clanY), Width = 150 };
            this.Controls.Add(txtClanIme);
            clanY += 40;

            lblClanPrezime = new Label { Text = "Prezime člana:", Location = new Point(20, clanY), AutoSize = true };
            this.Controls.Add(lblClanPrezime);
            txtClanPrezime = new TextBox { Location = new Point(120, clanY), Width = 150 };
            this.Controls.Add(txtClanPrezime);
            clanY += 40;

            lblClanEmail = new Label { Text = "Email:", Location = new Point(20, clanY), AutoSize = true };
            this.Controls.Add(lblClanEmail);
            txtClanEmail = new TextBox { Location = new Point(120, clanY), Width = 150 };
            this.Controls.Add(txtClanEmail);
            clanY += 40;

            lblClanTelefon = new Label { Text = "Telefon:", Location = new Point(20, clanY), AutoSize = true };
            this.Controls.Add(lblClanTelefon);
            txtClanTelefon = new TextBox { Location = new Point(120, clanY), Width = 150 };
            this.Controls.Add(txtClanTelefon);
            clanY += 40;

            lblClanJoin = new Label { Text = "Datum članidbe:", Location = new Point(20, clanY), AutoSize = true };
            this.Controls.Add(lblClanJoin);
            dtpClanJoin = new DateTimePicker
            {
                Location = new Point(140, clanY),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy",
                Width = 130
            };
            this.Controls.Add(dtpClanJoin);
            clanY += 40;

            btnDodajClana = new Button
            {
                Text = "Dodaj člana",
                Location = new Point(20, clanY),
                Size = new Size(250, 35)
            };
            btnDodajClana.Click += btnDodajClana_Click;
            this.Controls.Add(btnDodajClana);
            clanY += 40;

            var btnObrisiClana = new Button
            {
                Text = "Obriši člana",
                Location = new Point(20, clanY),
                Size = new Size(250, 35)
            };
            btnObrisiClana.Click += btnObrisiClana_Click;
            this.Controls.Add(btnObrisiClana);
            clanY += 40;

            var btnUrediClana = new Button
            {
                Text = "Spasi izmjene člana",
                Location = new Point(20, clanY),
                Size = new Size(250, 35)
            };
            btnUrediClana.Click += btnUrediClana_Click;
            this.Controls.Add(btnUrediClana);
            clanY += 40;

            var btnPonistiClana = new Button
            {
                Text = "Poništi odabir člana",
                Location = new Point(20, clanY),
                Size = new Size(250, 35)
            };
            btnPonistiClana.Click += btnPonistiClana_Click;
            this.Controls.Add(btnPonistiClana);

            dataGridClanovi = new DataGridView
            {
                Location = new Point(300, 720),
                Size = new Size(450, 280),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dataGridClanovi.SelectionChanged += dataGridClanovi_SelectionChanged;
            this.Controls.Add(dataGridClanovi);

            // ===== POSUDBE =====
            int posY = 1120;

            lblPosClan = new Label { Text = "Član:", Location = new Point(20, posY), AutoSize = true };
            this.Controls.Add(lblPosClan);
            cmbPosClan = new ComboBox
            {
                Location = new Point(120, posY),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbPosClan);
            posY += 40;

            lblPosKnjiga = new Label { Text = "Knjiga:", Location = new Point(20, posY), AutoSize = true };
            this.Controls.Add(lblPosKnjiga);
            cmbPosKnjiga = new ComboBox
            {
                Location = new Point(120, posY),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbPosKnjiga);
            posY += 40;

            lblPosDatum = new Label { Text = "Datum posudbe:", Location = new Point(20, posY), AutoSize = true };
            this.Controls.Add(lblPosDatum);
            dtpPosDatum = new DateTimePicker
            {
                Location = new Point(140, posY),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy",
                Width = 130
            };
            this.Controls.Add(dtpPosDatum);
            posY += 40;

            lblPosPovratak = new Label { Text = "Rok povratka:", Location = new Point(20, posY), AutoSize = true };
            this.Controls.Add(lblPosPovratak);
            dtpPosPovratak = new DateTimePicker
            {
                Location = new Point(140, posY),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy",
                Width = 130
            };
            this.Controls.Add(dtpPosPovratak);
            posY += 40;

            btnDodajPosudbu = new Button
            {
                Text = "Dodaj posudbu",
                Location = new Point(20, posY),
                Size = new Size(250, 35)
            };
            btnDodajPosudbu.Click += btnDodajPosudbu_Click;
            this.Controls.Add(btnDodajPosudbu);
            posY += 40;

            var btnVratiPosudbu = new Button
            {
                Text = "Označi kao vraćeno",
                Location = new Point(20, posY),
                Size = new Size(250, 35)
            };
            btnVratiPosudbu.Click += btnVratiPosudbu_Click;
            this.Controls.Add(btnVratiPosudbu);
            posY += 40;

            var btnObrisiPosudbu = new Button
            {
                Text = "Obriši posudbu",
                Location = new Point(20, posY),
                Size = new Size(250, 35)
            };
            btnObrisiPosudbu.Click += btnObrisiPosudbu_Click;
            this.Controls.Add(btnObrisiPosudbu);
            posY += 40;

            var btnPonistiPosudbu = new Button
            {
                Text = "Poništi odabir posudbe",
                Location = new Point(20, posY),
                Size = new Size(250, 35)
            };
            btnPonistiPosudbu.Click += btnPonistiPosudbu_Click;
            this.Controls.Add(btnPonistiPosudbu);

            dataGridPosudbe = new DataGridView
            {
                Location = new Point(300, 1120),
                Size = new Size(450, 300),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dataGridPosudbe.SelectionChanged += dataGridPosudbe_SelectionChanged;
            this.Controls.Add(dataGridPosudbe);

            lblStatusPosudbe = new Label
            {
                Location = new Point(300, 1430),
                AutoSize = true,
                Text = "Status: -"
            };
            this.Controls.Add(lblStatusPosudbe);
        }

        // ===== AUTORI – BAZA =====
        private void LoadAutori()
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = "SELECT AuthorId, FirstName, LastName, Country, BirthYear FROM Autori";
            var da = new SqlDataAdapter(q, conn);
            var dt = new DataTable();
            da.Fill(dt);
            dataGridAutori.DataSource = dt;

            if (dataGridAutori.Columns["AuthorId"] != null)
            {
                dataGridAutori.Columns["AuthorId"].HeaderText = "ID";
                dataGridAutori.Columns["FirstName"].HeaderText = "Ime";
                dataGridAutori.Columns["LastName"].HeaderText = "Prezime";
                dataGridAutori.Columns["Country"].HeaderText = "Država";
                dataGridAutori.Columns["BirthYear"].HeaderText = "God. rođenja";
            }
        }

        private void dataGridAutori_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridAutori.SelectedRows.Count == 0)
            {
                selectedAuthorId = null;
                txtIme.Clear(); txtPrezime.Clear(); txtDrzava.Clear(); txtGodina.Clear();
                return;
            }

            var row = dataGridAutori.SelectedRows[0];
            selectedAuthorId = Convert.ToInt32(row.Cells["AuthorId"].Value);
            txtIme.Text = row.Cells["FirstName"].Value?.ToString();
            txtPrezime.Text = row.Cells["LastName"].Value?.ToString();
            txtDrzava.Text = row.Cells["Country"].Value?.ToString();
            txtGodina.Text = row.Cells["BirthYear"].Value?.ToString();
        }

        private void btnDodajAutor_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIme.Text) || string.IsNullOrWhiteSpace(txtPrezime.Text))
            {
                MessageBox.Show("Ime i prezime su obavezni.");
                return;
            }

            int? godina = null;
            if (!string.IsNullOrWhiteSpace(txtGodina.Text))
            {
                if (int.TryParse(txtGodina.Text, out int g)) godina = g;
                else { MessageBox.Show("Godina rođenja mora biti broj."); return; }
            }

            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = "INSERT INTO Autori (FirstName, LastName, Country, BirthYear) VALUES (@f,@l,@c,@y)";
            using var cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@f", txtIme.Text.Trim());
            cmd.Parameters.AddWithValue("@l", txtPrezime.Text.Trim());
            cmd.Parameters.AddWithValue("@c",
                string.IsNullOrWhiteSpace(txtDrzava.Text) ? (object)DBNull.Value : txtDrzava.Text.Trim());
            cmd.Parameters.AddWithValue("@y", (object?)godina ?? DBNull.Value);
            cmd.ExecuteNonQuery();

            LoadAutori();
            LoadAutoriCombo();
            txtIme.Clear(); txtPrezime.Clear(); txtDrzava.Clear(); txtGodina.Clear();
        }

        private void btnObrisi_Click(object sender, EventArgs e)
        {
            if (dataGridAutori.SelectedRows.Count == 0)
            {
                MessageBox.Show("Odaberite autora.");
                return;
            }

            int id = Convert.ToInt32(dataGridAutori.SelectedRows[0].Cells["AuthorId"].Value);

            if (MessageBox.Show("Obrisati autora?", "Potvrda",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = "DELETE FROM Autori WHERE AuthorId=@id";
            using var cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            LoadAutori();
            LoadAutoriCombo();
        }

        private void btnUredi_Click(object sender, EventArgs e)
        {
            if (selectedAuthorId == null)
            {
                MessageBox.Show("Nije odabran autor.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtIme.Text) || string.IsNullOrWhiteSpace(txtPrezime.Text))
            {
                MessageBox.Show("Ime i prezime su obavezni.");
                return;
            }

            int? godina = null;
            if (!string.IsNullOrWhiteSpace(txtGodina.Text))
            {
                if (int.TryParse(txtGodina.Text, out int g)) godina = g;
                else { MessageBox.Show("Godina rođenja mora biti broj."); return; }
            }

            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = @"UPDATE Autori SET FirstName=@f, LastName=@l, Country=@c, BirthYear=@y WHERE AuthorId=@id";
            using var cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@f", txtIme.Text.Trim());
            cmd.Parameters.AddWithValue("@l", txtPrezime.Text.Trim());
            cmd.Parameters.AddWithValue("@c",
                string.IsNullOrWhiteSpace(txtDrzava.Text) ? (object)DBNull.Value : txtDrzava.Text.Trim());
            cmd.Parameters.AddWithValue("@y", (object?)godina ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@id", selectedAuthorId.Value);
            cmd.ExecuteNonQuery();

            LoadAutori();
            LoadAutoriCombo();
        }

        private void btnPonisti_Click(object sender, EventArgs e)
        {
            selectedAuthorId = null;
            txtIme.Clear(); txtPrezime.Clear(); txtDrzava.Clear(); txtGodina.Clear();
            dataGridAutori.ClearSelection();
        }

        // ===== KNJIGE – BAZA =====
        private void LoadKnjige()
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = @"
                SELECT k.BookId, k.Title, k.Year, k.ISBN, k.Quantity, k.AuthorId,
                       a.FirstName + ' ' + a.LastName AS Autor
                FROM Knjige k
                INNER JOIN Autori a ON k.AuthorId = a.AuthorId";
            var da = new SqlDataAdapter(q, conn);
            var dt = new DataTable();
            da.Fill(dt);
            dataGridKnjige.DataSource = dt;

            if (dataGridKnjige.Columns["BookId"] != null)
            {
                dataGridKnjige.Columns["BookId"].HeaderText = "ID";
                dataGridKnjige.Columns["Title"].HeaderText = "Naslov";
                dataGridKnjige.Columns["Year"].HeaderText = "Godina";
                dataGridKnjige.Columns["ISBN"].HeaderText = "ISBN";
                dataGridKnjige.Columns["Quantity"].HeaderText = "Količina";
                dataGridKnjige.Columns["Autor"].HeaderText = "Autor";
                dataGridKnjige.Columns["AuthorId"].Visible = false;
            }
        }

        private void LoadAutoriCombo()
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = "SELECT AuthorId, FirstName + ' ' + LastName AS ImePrezime FROM Autori";
            var da = new SqlDataAdapter(q, conn);
            var dt = new DataTable();
            da.Fill(dt);
            cmbAutorKnj.DataSource = dt;
            cmbAutorKnj.DisplayMember = "ImePrezime";
            cmbAutorKnj.ValueMember = "AuthorId";
        }

        private void btnDodajKnjigu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNaslov.Text))
            { MessageBox.Show("Naslov je obavezan."); return; }
            if (cmbAutorKnj.SelectedValue == null)
            { MessageBox.Show("Odaberite autora."); return; }

            int? godina = null;
            if (!string.IsNullOrWhiteSpace(txtGodinaKnj.Text))
            {
                if (!int.TryParse(txtGodinaKnj.Text, out int g))
                { MessageBox.Show("Godina mora biti broj."); return; }
                godina = g;
            }

            int? kolicina = null;
            if (!string.IsNullOrWhiteSpace(txtKolicina.Text))
            {
                if (!int.TryParse(txtKolicina.Text, out int k))
                { MessageBox.Show("Količina mora biti broj."); return; }
                kolicina = k;
            }

            int authorId = Convert.ToInt32(cmbAutorKnj.SelectedValue);

            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = @"INSERT INTO Knjige (Title, Year, ISBN, AuthorId, Quantity)
                         VALUES (@t,@y,@i,@a,@q)";
            using var cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@t", txtNaslov.Text.Trim());
            cmd.Parameters.AddWithValue("@y", (object?)godina ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@i",
                string.IsNullOrWhiteSpace(txtISBN.Text) ? (object)DBNull.Value : txtISBN.Text.Trim());
            cmd.Parameters.AddWithValue("@a", authorId);
            cmd.Parameters.AddWithValue("@q", (object?)kolicina ?? DBNull.Value);
            cmd.ExecuteNonQuery();

            LoadKnjige();
            txtNaslov.Clear(); txtGodinaKnj.Clear(); txtISBN.Clear(); txtKolicina.Clear();
        }

        private void dataGridKnjige_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridKnjige.SelectedRows.Count == 0)
            {
                selectedBookId = null;
                txtNaslov.Clear(); txtGodinaKnj.Clear(); txtISBN.Clear(); txtKolicina.Clear();
                return;
            }

            var row = dataGridKnjige.SelectedRows[0];
            selectedBookId = Convert.ToInt32(row.Cells["BookId"].Value);
            txtNaslov.Text = row.Cells["Title"].Value?.ToString();
            txtGodinaKnj.Text = row.Cells["Year"].Value?.ToString();
            txtISBN.Text = row.Cells["ISBN"].Value?.ToString();
            txtKolicina.Text = row.Cells["Quantity"].Value?.ToString();

            if (row.Cells["AuthorId"].Value != null)
                cmbAutorKnj.SelectedValue = row.Cells["AuthorId"].Value;
        }

        private void btnObrisiKnjigu_Click(object sender, EventArgs e)
        {
            if (dataGridKnjige.SelectedRows.Count == 0)
            { MessageBox.Show("Odaberite knjigu."); return; }

            int id = Convert.ToInt32(dataGridKnjige.SelectedRows[0].Cells["BookId"].Value);
            if (MessageBox.Show("Obrisati knjigu?", "Potvrda",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = "DELETE FROM Knjige WHERE BookId=@id";
            using var cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            LoadKnjige();
        }

        private void btnUrediKnjigu_Click(object sender, EventArgs e)
        {
            if (selectedBookId == null) { MessageBox.Show("Nije odabrana knjiga."); return; }
            if (string.IsNullOrWhiteSpace(txtNaslov.Text))
            { MessageBox.Show("Naslov je obavezan."); return; }
            if (cmbAutorKnj.SelectedValue == null)
            { MessageBox.Show("Odaberite autora."); return; }

            int? godina = null;
            if (!string.IsNullOrWhiteSpace(txtGodinaKnj.Text))
            {
                if (!int.TryParse(txtGodinaKnj.Text, out int g))
                { MessageBox.Show("Godina mora biti broj."); return; }
                godina = g;
            }

            int? kolicina = null;
            if (!string.IsNullOrWhiteSpace(txtKolicina.Text))
            {
                if (!int.TryParse(txtKolicina.Text, out int k))
                { MessageBox.Show("Količina mora biti broj."); return; }
                kolicina = k;
            }

            int authorId = Convert.ToInt32(cmbAutorKnj.SelectedValue);

            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = @"UPDATE Knjige SET Title=@t, Year=@y, ISBN=@i, AuthorId=@a, Quantity=@q
                         WHERE BookId=@id";
            using var cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@t", txtNaslov.Text.Trim());
            cmd.Parameters.AddWithValue("@y", (object?)godina ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@i",
                string.IsNullOrWhiteSpace(txtISBN.Text) ? (object)DBNull.Value : txtISBN.Text.Trim());
            cmd.Parameters.AddWithValue("@a", authorId);
            cmd.Parameters.AddWithValue("@q", (object?)kolicina ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@id", selectedBookId.Value);
            cmd.ExecuteNonQuery();

            LoadKnjige();
        }

        private void btnPonistiKnj_Click(object sender, EventArgs e)
        {
            selectedBookId = null;
            txtNaslov.Clear(); txtGodinaKnj.Clear(); txtISBN.Clear(); txtKolicina.Clear();
            dataGridKnjige.ClearSelection();
        }

        // ===== ČLANOVI – BAZA =====
        private void LoadClanovi()
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = "SELECT MemberId, FirstName, LastName, Email, Phone, JoinDate FROM Clanovi";
            var da = new SqlDataAdapter(q, conn);
            var dt = new DataTable();
            da.Fill(dt);
            dataGridClanovi.DataSource = dt;

            if (dataGridClanovi.Columns["MemberId"] != null)
            {
                dataGridClanovi.Columns["MemberId"].HeaderText = "ID";
                dataGridClanovi.Columns["FirstName"].HeaderText = "Ime";
                dataGridClanovi.Columns["LastName"].HeaderText = "Prezime";
                dataGridClanovi.Columns["Email"].HeaderText = "Email";
                dataGridClanovi.Columns["Phone"].HeaderText = "Telefon";
                dataGridClanovi.Columns["JoinDate"].HeaderText = "Učlanjen";
            }
        }

        private void dataGridClanovi_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridClanovi.SelectedRows.Count == 0)
            {
                selectedMemberId = null;
                txtClanIme.Clear(); txtClanPrezime.Clear(); txtClanEmail.Clear(); txtClanTelefon.Clear();
                return;
            }

            var row = dataGridClanovi.SelectedRows[0];
            selectedMemberId = Convert.ToInt32(row.Cells["MemberId"].Value);
            txtClanIme.Text = row.Cells["FirstName"].Value?.ToString();
            txtClanPrezime.Text = row.Cells["LastName"].Value?.ToString();
            txtClanEmail.Text = row.Cells["Email"].Value?.ToString();
            txtClanTelefon.Text = row.Cells["Phone"].Value?.ToString();

            if (row.Cells["JoinDate"].Value != null &&
                DateTime.TryParse(row.Cells["JoinDate"].Value.ToString(), out DateTime jd))
                dtpClanJoin.Value = jd;
        }

        private void btnDodajClana_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtClanIme.Text) || string.IsNullOrWhiteSpace(txtClanPrezime.Text))
            { MessageBox.Show("Ime i prezime su obavezni."); return; }

            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = @"INSERT INTO Clanovi (FirstName, LastName, Email, Phone, JoinDate)
                         VALUES (@f,@l,@e,@p,@d)";
            using var cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@f", txtClanIme.Text.Trim());
            cmd.Parameters.AddWithValue("@l", txtClanPrezime.Text.Trim());
            cmd.Parameters.AddWithValue("@e",
                string.IsNullOrWhiteSpace(txtClanEmail.Text) ? (object)DBNull.Value : txtClanEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@p",
                string.IsNullOrWhiteSpace(txtClanTelefon.Text) ? (object)DBNull.Value : txtClanTelefon.Text.Trim());
            cmd.Parameters.AddWithValue("@d", dtpClanJoin.Value.Date);
            cmd.ExecuteNonQuery();

            LoadClanovi();
            txtClanIme.Clear(); txtClanPrezime.Clear(); txtClanEmail.Clear(); txtClanTelefon.Clear();
        }

        private void btnObrisiClana_Click(object sender, EventArgs e)
        {
            if (dataGridClanovi.SelectedRows.Count == 0)
            { MessageBox.Show("Odaberite člana."); return; }

            int id = Convert.ToInt32(dataGridClanovi.SelectedRows[0].Cells["MemberId"].Value);
            if (MessageBox.Show("Obrisati člana?", "Potvrda",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = "DELETE FROM Clanovi WHERE MemberId=@id";
            using var cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            LoadClanovi();
        }

        private void btnUrediClana_Click(object sender, EventArgs e)
        {
            if (selectedMemberId == null) { MessageBox.Show("Nije odabran član."); return; }
            if (string.IsNullOrWhiteSpace(txtClanIme.Text) || string.IsNullOrWhiteSpace(txtClanPrezime.Text))
            { MessageBox.Show("Ime i prezime su obavezni."); return; }

            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = @"UPDATE Clanovi SET FirstName=@f, LastName=@l, Email=@e,
                         Phone=@p, JoinDate=@d WHERE MemberId=@id";
            using var cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@f", txtClanIme.Text.Trim());
            cmd.Parameters.AddWithValue("@l", txtClanPrezime.Text.Trim());
            cmd.Parameters.AddWithValue("@e",
                string.IsNullOrWhiteSpace(txtClanEmail.Text) ? (object)DBNull.Value : txtClanEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@p",
                string.IsNullOrWhiteSpace(txtClanTelefon.Text) ? (object)DBNull.Value : txtClanTelefon.Text.Trim());
            cmd.Parameters.AddWithValue("@d", dtpClanJoin.Value.Date);
            cmd.Parameters.AddWithValue("@id", selectedMemberId.Value);
            cmd.ExecuteNonQuery();

            LoadClanovi();
        }

        private void btnPonistiClana_Click(object sender, EventArgs e)
        {
            selectedMemberId = null;
            txtClanIme.Clear(); txtClanPrezime.Clear(); txtClanEmail.Clear(); txtClanTelefon.Clear();
            dataGridClanovi.ClearSelection();
        }

        // ===== POSUDBE – BAZA =====
        private void LoadClanoviComboPosudbe()
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = "SELECT MemberId, FirstName + ' ' + LastName AS ImePrezime FROM Clanovi";
            var da = new SqlDataAdapter(q, conn);
            var dt = new DataTable();
            da.Fill(dt);
            cmbPosClan.DataSource = dt;
            cmbPosClan.DisplayMember = "ImePrezime";
            cmbPosClan.ValueMember = "MemberId";
        }

        private void LoadKnjigeComboPosudbe()
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = "SELECT BookId, Title FROM Knjige";
            var da = new SqlDataAdapter(q, conn);
            var dt = new DataTable();
            da.Fill(dt);
            cmbPosKnjiga.DataSource = dt;
            cmbPosKnjiga.DisplayMember = "Title";
            cmbPosKnjiga.ValueMember = "BookId";
        }

        private void LoadPosudbe()
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = @"
                SELECT p.LoanId, p.MemberId, p.BookId,
                       c.FirstName + ' ' + c.LastName AS Clan,
                       k.Title AS Knjiga,
                       p.LoanDate,
                       p.DueDate,
                       p.ReturnDate
                FROM Posudbe p
                INNER JOIN Clanovi c ON p.MemberId = c.MemberId
                INNER JOIN Knjige k ON p.BookId = k.BookId";
            var da = new SqlDataAdapter(q, conn);
            var dt = new DataTable();
            da.Fill(dt);
            dataGridPosudbe.DataSource = dt;

            if (dataGridPosudbe.Columns["LoanId"] != null)
            {
                dataGridPosudbe.Columns["LoanId"].HeaderText = "ID";
                dataGridPosudbe.Columns["Clan"].HeaderText = "Član";
                dataGridPosudbe.Columns["Knjiga"].HeaderText = "Knjiga";
                dataGridPosudbe.Columns["LoanDate"].HeaderText = "Datum posudbe";
                dataGridPosudbe.Columns["DueDate"].HeaderText = "Rok povratka";
                dataGridPosudbe.Columns["ReturnDate"].HeaderText = "Datum povratka";

                dataGridPosudbe.Columns["MemberId"].Visible = false;
                dataGridPosudbe.Columns["BookId"].Visible = false;
            }
        }

        private void dataGridPosudbe_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridPosudbe.SelectedRows.Count == 0)
            {
                selectedLoanId = null;
                ResetPosudbeFields();
                lblStatusPosudbe.Text = "Status: -";
                return;
            }

            var row = dataGridPosudbe.SelectedRows[0];
            selectedLoanId = Convert.ToInt32(row.Cells["LoanId"].Value);

            if (row.Cells["MemberId"].Value != null)
                cmbPosClan.SelectedValue = row.Cells["MemberId"].Value;
            if (row.Cells["BookId"].Value != null)
                cmbPosKnjiga.SelectedValue = row.Cells["BookId"].Value;

            DateTime? loanDate = null;
            DateTime? dueDate = null;
            DateTime? returnDate = null;

            if (row.Cells["LoanDate"].Value != null &&
                DateTime.TryParse(row.Cells["LoanDate"].Value.ToString(), out DateTime ld))
            {
                loanDate = ld;
                dtpPosDatum.Value = ld;
            }

            if (row.Cells["DueDate"].Value != null &&
                DateTime.TryParse(row.Cells["DueDate"].Value.ToString(), out DateTime dd))
            {
                dueDate = dd;
                dtpPosPovratak.Value = dd;
            }
            else
            {
                dtpPosPovratak.Value = DateTime.Today;
            }

            if (row.Cells["ReturnDate"].Value != null &&
                row.Cells["ReturnDate"].Value != DBNull.Value &&
                DateTime.TryParse(row.Cells["ReturnDate"].Value.ToString(), out DateTime rd))
            {
                returnDate = rd;
            }

            // Status kašnjenja
            if (dueDate == null)
            {
                lblStatusPosudbe.Text = "Status: rok nije definisan.";
                return;
            }

            if (returnDate == null)
            {
                if (DateTime.Today > dueDate.Value)
                {
                    int kasni = (DateTime.Today - dueDate.Value).Days;
                    lblStatusPosudbe.Text = $"Status: još nije vraćena (kasni {kasni} dana).";
                }
                else
                {
                    int preostalo = (dueDate.Value - DateTime.Today).Days;
                    lblStatusPosudbe.Text = $"Status: rok ističe za {preostalo} dana.";
                }
            }
            else
            {
                if (returnDate.Value > dueDate.Value)
                {
                    int kasni = (returnDate.Value - dueDate.Value).Days;
                    lblStatusPosudbe.Text = $"Status: vraćena sa zakašnjenjem {kasni} dana.";
                }
                else
                {
                    lblStatusPosudbe.Text = "Status: vraćena na vrijeme.";
                }
            }
        }

        private void btnDodajPosudbu_Click(object sender, EventArgs e)
        {
            if (cmbPosClan.SelectedValue == null) { MessageBox.Show("Odaberite člana."); return; }
            if (cmbPosKnjiga.SelectedValue == null) { MessageBox.Show("Odaberite knjigu."); return; }

            int memberId = Convert.ToInt32(cmbPosClan.SelectedValue);
            int bookId = Convert.ToInt32(cmbPosKnjiga.SelectedValue);
            DateTime loanDate = dtpPosDatum.Value.Date;
            DateTime dueDate = dtpPosPovratak.Value.Date;

            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = @"INSERT INTO Posudbe (MemberId, BookId, LoanDate, DueDate, ReturnDate)
                         VALUES (@m, @b, @loan, @due, NULL)";
            using var cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@m", memberId);
            cmd.Parameters.AddWithValue("@b", bookId);
            cmd.Parameters.AddWithValue("@loan", loanDate);
            cmd.Parameters.AddWithValue("@due", dueDate);
            cmd.ExecuteNonQuery();

            LoadPosudbe();
            ResetPosudbeFields();
            MessageBox.Show("Posudba je dodana.", "Info");
        }

        private void btnVratiPosudbu_Click(object sender, EventArgs e)
        {
            if (selectedLoanId == null) { MessageBox.Show("Nije odabrana posudba."); return; }

            DateTime returnDate = DateTime.Today;

            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = @"UPDATE Posudbe SET ReturnDate=@r WHERE LoanId=@id";
            using var cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@r", returnDate);
            cmd.Parameters.AddWithValue("@id", selectedLoanId.Value);
            cmd.ExecuteNonQuery();

            LoadPosudbe();
        }

        private void btnObrisiPosudbu_Click(object sender, EventArgs e)
        {
            if (dataGridPosudbe.SelectedRows.Count == 0)
            { MessageBox.Show("Odaberite posudbu."); return; }

            int id = Convert.ToInt32(dataGridPosudbe.SelectedRows[0].Cells["LoanId"].Value);
            if (MessageBox.Show("Obrisati posudbu?", "Potvrda",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            using var conn = new SqlConnection(connectionString);
            conn.Open();
            string q = "DELETE FROM Posudbe WHERE LoanId=@id";
            using var cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            LoadPosudbe();
            ResetPosudbeFields();
        }

        private void btnPonistiPosudbu_Click(object sender, EventArgs e)
        {
            selectedLoanId = null;
            ResetPosudbeFields();
            dataGridPosudbe.ClearSelection();
        }

        private void ResetPosudbeFields()
        {
            if (cmbPosClan.Items.Count > 0) cmbPosClan.SelectedIndex = 0;
            if (cmbPosKnjiga.Items.Count > 0) cmbPosKnjiga.SelectedIndex = 0;
            dtpPosDatum.Value = DateTime.Today;
            dtpPosPovratak.Value = DateTime.Today;
            if (lblStatusPosudbe != null)
                lblStatusPosudbe.Text = "Status: -";
        }
    }
}
