﻿using System.Runtime.InteropServices;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Writers;

namespace CUE4Parse.UE4.Objects.MovieScene;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct FMovieSceneEvaluationTreeNode : IUStruct, ISerializable
{
    /** The time-range that this node represents */
    public readonly TRange<FFrameNumber> Range;
    /** Handle to the parent node */
    public readonly FMovieSceneEvaluationTreeNodeHandle Parent;
    /** Identifier for the child node entries associated with this node (FMovieSceneEvaluationTree::ChildNodes) */
    public readonly FEvaluationTreeEntryHandle ChildrenID;
    /** Identifier for externally stored data entries associated with this node */
    public readonly FEvaluationTreeEntryHandle DataID;

    public void Serialize(FArchiveWriter Ar)
    {
        Ar.Serialize(Range);
        Ar.Serialize(Parent);
        Ar.Serialize(ChildrenID);
        Ar.Serialize(DataID);
    }
}