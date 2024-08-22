using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// Classe que representa uma ação que pode ser executada por um jogador
/// E enviada para o servidor
/// </summary>
public class Action
{
    // Action type
    public string type;

    // ID of the player that executed the action
    public string actor;

    // parametrs
    public Dictionary<string, string> parameters;

    /// Conversion of a JSON to the Action object
    public static Action FromJson(string json) {
        Action action = JsonUtility.FromJson<Action>(json);
        return action;
    }

    /// Conversion of the Action object for a JSON
    public string ToJson() {
        return JsonConvert.SerializeObject(this);
    }
}
