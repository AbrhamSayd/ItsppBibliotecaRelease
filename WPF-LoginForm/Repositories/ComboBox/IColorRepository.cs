using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBiblioteca.Models;
using WPFBiblioteca.Models.ComboBoxModels;

namespace WPFBiblioteca.Repositories.ComboBox
{
    public interface IColorRepository
    {
        Task<string> Add(ColorModel color);
        Task<string> Edit(ColorModel color, int id);
        Task<string> Delete(int id);
        Task<ColorModel> GetById(int id);
        Task<IEnumerable<ColorModel>> GetByAll();
        string GetError();
    }
}
