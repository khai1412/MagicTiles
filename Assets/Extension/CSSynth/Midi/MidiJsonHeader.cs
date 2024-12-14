using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiJsonHeader
{
    public int isCrypto = 0;
    public float gameSpeedIncrease = 0f;
    public string str_SongClip = string.Empty;
    public string str_SongSpeed = string.Empty;
    public float beginDistanceBySecond;
    public float addExtraDistance;
    public bool isChangePitch = false;
    public float noteTimeUnit = 0;
}
