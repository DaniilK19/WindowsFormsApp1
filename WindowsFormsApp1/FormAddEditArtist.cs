using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class FormAddEditArtist : Form
    {
        private readonly ArtistsService _service;
        private Artist _artist;
        public bool IsNewArtist { get; private set; }
        // Властивість для доступу до художника з інших форм
        public Artist Artist { get; private set; }


        public FormAddEditArtist()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.FormAddEditArtist_Load);
            Artist = new Artist(); // Створення нового об'єкта художника
            _service = new ArtistsService();
            _artist = new Artist(); // Створюємо новий об'єкт художника
            IsNewArtist = true; // Позначаємо, що це новий художник
            txtName.Location = new Point(10, 10); // Встановіть потрібне розташування
            txtName.Size = new Size(200, 20); // Встановіть потрібний розмір
            this.Controls.Add(txtName); // Додайте txtName до форми

            // Налаштування txtBio
            txtBio.Location = new Point(10, 40); // Встановіть потрібне розташування
            txtBio.Size = new Size(200, 60); // Встановіть потрібний розмір
            txtBio.Multiline = true; // Дозволити багаторядковий ввід
            this.Controls.Add(txtBio); // Додайте txtBio до форми
        }

        // Конструктор для редагування існуючого художника
        public FormAddEditArtist(Artist artist) : this()
        {
            // Перевірка, що об'єкт artist не є null
            _artist = artist ?? throw new ArgumentNullException(nameof(artist));
            IsNewArtist = false;
        }

        private void FormAddEditArtist_Load(object sender, EventArgs e)
        {
            if (!IsNewArtist)
            {
                // Якщо редагуємо художника, заповнюємо поля даними
                txtName.Text = _artist.Name;
                txtBio.Text = _artist.Bio;
                // Заповніть інші поля, якщо вони є
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Збір інформації з елементів управління
            _artist.Name = txtName.Text;
            _artist.Bio = txtBio.Text;
            // Збір іншої інформації

            if (IsNewArtist)
            {
                _service.AddArtist(_artist);
            }
            else
            {
                _service.UpdateArtist(_artist);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }


}
