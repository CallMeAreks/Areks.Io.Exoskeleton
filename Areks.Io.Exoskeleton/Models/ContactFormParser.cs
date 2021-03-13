using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Areks.Io.Exoskeleton.Models
{
    public class ContactFormParser
    {
        private IDictionary<string, string> Fields { get; set; }
        public bool Succeeded => Fields.Any();
        private const string Separator = "\n ————————— \n";
        
        public ContactFormParser(IFormCollection collection)
        {
            Fields = new Dictionary<string, string>();
            Parse(collection);
        }

        public override string ToString()
        {
            return $"{Separator} {string.Join(Separator, Fields.Select(f => $"*{f.Key}*:\n{f.Value}"))}";
        }

        private void Parse(IFormCollection collection)
        {
            var formFields = collection.Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
                .ToDictionary(item => item.Key, item => item.Value.ToString());

            if (formFields.Any(InvalidField))
            {
                return;
            }

            Fields = formFields;
        }

        private bool InvalidField(KeyValuePair<string, string> keyValuePair)
        {
            return keyValuePair.Key.EndsWith("_ignore");
        }
    }
}