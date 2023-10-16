using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

[Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct MUIInitComponent : IComponent
{
    // just a TAG component to make a `Init` flag for initializers
}