using System;
using UnityEngine;

[Serializable]
public struct TaskID
{
    [SerializeField]
    public string ID;

    public TaskID(string id) => ID = id;

    public static implicit operator string(TaskID taskID) => taskID.ID;
    public static implicit operator TaskID(string id) => new TaskID(id);

    public override bool Equals(object obj)
    {
        if (obj is TaskID other) return Equals(other);
        if (obj is string str) return Equals(new TaskID(str));
        return false;
    }

    public bool Equals(TaskID other) =>
        string.Equals(Normalize(ID), Normalize(other.ID), StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => Normalize(ID).GetHashCode();

    public override string ToString() => ID;

    private static string Normalize(string s) =>
        string.IsNullOrWhiteSpace(s) ? "" : s.ToLowerInvariant().Replace(" ", "");

    public static bool operator ==(TaskID a, TaskID b) => a.Equals(b);
    public static bool operator !=(TaskID a, TaskID b) => !a.Equals(b);
}
