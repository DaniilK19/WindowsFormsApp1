using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class FormArtists : Form
    {
        private ArtistsService _service = new ArtistsService();
        private Artist _selectedArtist;

        public FormArtists()
        {
            InitializeComponent();
            _service = new ArtistsService();
            dataGridView1.AutoGenerateColumns = false;
            InitializeDataGridViewColumns();
            UpdateDataGridView(); // Завантаження та відображення даних
        }
        private void InitializeDataGridViewColumns()
        {
            // Очищаємо існуючі колонки
            dataGridView1.Columns.Clear();

            // Додаємо колонку ID
            var idColumn = new DataGridViewTextBoxColumn();
            idColumn.Name = "ArtistId";
            idColumn.HeaderText = "ID";
            idColumn.DataPropertyName = "ArtistId"; // властивість моделі даних
            dataGridView1.Columns.Add(idColumn);

            // Додаємо текстову колонку Name
            var nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.Name = "Name";
            nameColumn.HeaderText = "Name";
            nameColumn.DataPropertyName = "Name"; // властивість моделі даних
            dataGridView1.Columns.Add(nameColumn);

            // Додаємо текстову колонку Bio
            var bioColumn = new DataGridViewTextBoxColumn();
            bioColumn.Name = "Bio";
            bioColumn.HeaderText = "Bio";
            bioColumn.DataPropertyName = "Bio"; // властивість моделі даних
            dataGridView1.Columns.Add(bioColumn);

            // Додаємо колонку зображення ArtworkImage
            var imageColumn = new DataGridViewImageColumn();
            imageColumn.Name = "ArtworkImage";
            imageColumn.HeaderText = "Artwork Image";
            imageColumn.DataPropertyName = "ArtworkImage"; // властивість моделі даних
            dataGridView1.Columns.Add(imageColumn);

            // Встановлюємо режим відображення зображень
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imageColumn.Width = 100; // Встановлюємо бажану ширину колонки

            // ... можна додати інші колонки, якщо потрібно ...
        }
        private void FormArtists_Load(object sender, EventArgs e)
        {
            ArtistsService service = new ArtistsService();
            dataGridView1.DataSource = service.GetArtists();
        }

        private void btnAddArtist_Click(object sender, EventArgs e)
        {
            var addForm = new FormAddEditArtist(); // Припускаємо, що така форма існує
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                ArtistsService service = new ArtistsService();
                service.AddArtist(addForm.Artist); // addForm.Artist - це об'єкт Artist, який був створений на формі
                UpdateDataGridView(); // Оновлення DataGridView
            }
        }

        private void btnEditArtist_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int artistId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ArtistId"].Value);
                // Спочатку отримайте художника з бази даних за допомогою artistId.
                // Це приклад, і вам потрібно використати реальний метод з ArtistsService для отримання художника.
                Artist artistToEdit = _service.GetArtistById(artistId);

                // Тепер створіть і покажіть форму редагування художника.
                var editForm = new FormAddEditArtist(artistToEdit);

                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    // Оновіть художника в базі даних.
                    _service.UpdateArtist(editForm.Artist);
                    // Оновіть відображення DataGridView.
                    UpdateDataGridView();
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            UpdateDataGridView();
        }

        private void btnDeleteArtist_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null &&
        MessageBox.Show("Видалити обраного художника?", "Підтвердження", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int artistId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ArtistId"].Value);
                ArtistsService service = new ArtistsService();
                service.DeleteArtist(artistId);
                UpdateDataGridView();
            }
        }

        private void UpdateDataGridView()
        {
            // Get the latest data from the database.
            var artistsData = _service.GetArtists();
            var bindingList = new BindingList<ArtistViewModel>(artistsData.Select(artist => new ArtistViewModel
            {
                ArtistId = artist.ArtistId,
                Name = artist.Name,
                Bio = artist.Bio,
                ArtworkImage = artist.ImageData != null ? Image.FromStream(new MemoryStream(artist.ImageData)) : null
            }).ToList());

            dataGridView1.DataSource = bindingList;

            // Make sure your DataGridView has a column of type DataGridViewImageColumn for the images.
            // Adjust settings for that column if necessary.
            var imageColumn = dataGridView1.Columns["ArtworkImage"] as DataGridViewImageColumn;
            if (imageColumn != null)
            {
                imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
                imageColumn.Width = 100; // Set your desired width
            }
        }

        // Helper method to convert a byte array to an image.
        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }

        public class ArtistViewModel
        {
            public int ArtistId { get; set; }
            public string Name { get; set; }
            public string Bio { get; set; }
            public Image ArtworkImage { get; set; } // Переконайтеся, що ця властивість є в класі
        }

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            // Перевіряємо, чи вибрано художника
            if (dataGridView1.CurrentRow != null)
            {
                int artistId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ArtistId"].Value);
                Artist selectedArtist = _service.GetArtistById(artistId);

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = "Виберіть зображення";
                    openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Читаємо файл і конвертуємо його в масив байтів
                        string filePath = openFileDialog.FileName;
                        selectedArtist.ImageData = File.ReadAllBytes(filePath);

                        // Оновлюємо художника в базі даних
                        _service.UpdateArtist(selectedArtist);

                        // Оновлюємо DataGridView
                        UpdateDataGridView();
                    }
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть художника зі списку.", "Художник не вибрано");
            }
        }
        private Artist GetSelectedArtistFromDataGridView()
        {
            // This should contain the logic to get the selected artist from the DataGridView
            // For example:
            // int artistId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ArtistId"].Value);
            // return _service.GetArtistById(artistId);
            return new Artist(); // Placeholder, return an actual artist object
        }


        private void btnClearAll_Click(object sender, EventArgs e)
        {
            // Confirm before clearing all artists
            if (MessageBox.Show("Are you sure you want to clear all artists?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _service.ClearAllArtists(); // Call the method in the service
                UpdateDataGridView();
            }
        }


    }
}
