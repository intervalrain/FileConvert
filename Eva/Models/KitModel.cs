using System;
namespace Eva.Models;

public record KitModel(int Id, int Sid, string Text, KitType Type, string OptText, string OptValue, string Annotation, string Note);

public enum KitType
{
    Single = 0,
    Multi = 1,
    Number = 2,
    String = 3,
    Time = 4,
    Location = 5,
    Set = 9,
    NextPage = 10
}