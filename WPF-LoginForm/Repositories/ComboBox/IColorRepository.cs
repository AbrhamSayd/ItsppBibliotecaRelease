using System.Collections.Generic;
using System.Threading.Tasks;
using WPFBiblioteca.Models.ComboBoxModels;

namespace WPFBiblioteca.Repositories.ComboBox;

public interface IColorRepository
{
    Task<string> Add(ColorModel color);
    Task<string> Edit(ColorModel color, int id);
    Task<string> Delete(int id);
    Task<ColorModel> GetById(int id);
    Task<IEnumerable<ColorModel>> GetByAll();
    string GetError();
}