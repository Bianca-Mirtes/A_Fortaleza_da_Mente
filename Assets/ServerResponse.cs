using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que representa uma resposta do servidor
/// </summary>
public class ServerResponse
{
    // Response type
    public string type { get; set; }

    // Parâmetros da resposta (pode ser qualquer coisa armazenada em um dicionário)
    public Dictionary<string, string> payload { get; set; }

    ServerResponse()
    {
        payload = new Dictionary<string, string>();
    }

    // Tostring mostrando tudo
    public override string ToString()
    {
        string str = "";
        str += "Type: " + type + "\n";
        str += "Payload: " + payload + "\n";
        return str;
    }
}
