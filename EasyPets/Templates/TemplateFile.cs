using System.Collections.Generic;

namespace EasyPets.EasyPets.Templates;

public class TemplateFile
{
    public string? ModSource { get; set; }
    public List<PetTemplate> Pets { get; set; } = new();

}