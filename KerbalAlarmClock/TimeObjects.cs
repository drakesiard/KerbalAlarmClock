﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using KSP;

namespace KerbalAlarmClock
{
    /// <summary>
    /// A class to store the UT of events and get back useful data
    /// </summary>
    public class KACTime
    {

        //really there are 31,446,925.9936 seconds in a year, use 365*24 so the reciprocal math 
        //  to go to years and get back to full days isnt confusing - have to sort this out some day though.
        //NOTE: KSP Dates appear to all be 365 * 24 as well - no fractions - woohoo
        const double HoursPerYearEarth = 365 * 24;

        #region "Constructors"
        public KACTime()
        { }
        public KACTime(double NewUT)
        {
            UT = NewUT;
        }
        public KACTime(double Years, double Days, double Hours, double Minutes, double Seconds)
        {
            UT = KACTime.BuildUTFromRaw(Years, Days, Hours, Minutes, Seconds);
        }
        #endregion

        /// <summary>
        /// Build the UT from raw values
        /// </summary>
        /// <param name="Years"></param>
        /// <param name="Days"></param>
        /// <param name="Hours"></param>
        /// <param name="Minutes"></param>
        /// <param name="Seconds"></param>
        public void BuildUT(double Years, double Days, double Hours, double Minutes, double Seconds)
        {
            UT = KACTime.BuildUTFromRaw(Years, Days, Hours, Minutes, Seconds);
        }

        public void BuildUT(String Years, String Days, String Hours, String Minutes, String Seconds)
        {
            BuildUT(Convert.ToDouble(Years), Convert.ToDouble(Days), Convert.ToDouble(Hours), Convert.ToDouble(Minutes), Convert.ToDouble(Seconds));
        }

        #region "Properties"

        //Stores the Universal Time in game seconds
        public double UT;

        //readonly props that resolve from UT
        public long Second
        {
            get { return Convert.ToInt64(Math.Truncate(UT % 60)); }
        }
        public long Minute
        {
            get { return Convert.ToInt64(Math.Truncate((UT / 60) % 60)); }
        }

        private double HourRaw { get { return UT / 60 / 60; } }
        public long HourEarth
        {
            get { return Convert.ToInt64(Math.Truncate(HourRaw % 24)); }
        }

        public long DayEarth
        {
            get { return Convert.ToInt64(Math.Truncate(((HourRaw % HoursPerYearEarth) / 24))); }
        }

        public long YearEarth
        {
            get { return Convert.ToInt64(Math.Truncate((HourRaw / HoursPerYearEarth))); }
        }
        #endregion

        #region "String Formatting"
        public String IntervalString()
        {
            return IntervalString(6);
        }
        public String IntervalString(int segments)
        {
            String strReturn = "";

            if (UT < 0) strReturn += "+ ";

            int intUsed = 0;

            if (intUsed < segments && YearEarth != 0)
            {
                strReturn += String.Format("{0}y", Math.Abs(YearEarth));
                intUsed++;
            }

            if (intUsed < segments && (DayEarth != 0 || intUsed > 0))
            {
                if (intUsed > 0) strReturn += ", ";
                strReturn += String.Format("{0}d", Math.Abs(DayEarth));
                intUsed++;
            }

            if (intUsed < segments && (HourEarth != 0 || intUsed > 0))
            {
                if (intUsed > 0) strReturn += ", ";
                strReturn += String.Format("{0}h", Math.Abs(HourEarth));
                intUsed++;
            }
            if (intUsed < segments && (Minute != 0 || intUsed > 0))
            {
                if (intUsed > 0) strReturn += ", ";
                strReturn += String.Format("{0}m", Math.Abs(Minute));
                intUsed++;
            }
            if (intUsed < segments)// && (Second != 0 || intUsed > 0))
            {
                if (intUsed > 0) strReturn += ", ";
                strReturn += String.Format("{0}s", Math.Abs(Second));
                intUsed++;
            }


            return strReturn;
        }

        public String IntervalDateTimeString()
        {
            return IntervalDateTimeString(6);
        }
        public String IntervalDateTimeString(int segments)
        {
            String strReturn = "";

            if (UT < 0) strReturn += "+ ";

            int intUsed = 0;

            if (intUsed < segments && YearEarth != 0)
            {
                strReturn += String.Format("{0}y", Math.Abs(YearEarth));
                intUsed++;
            }

            if (intUsed < segments && (DayEarth != 0 || intUsed > 0))
            {
                if (intUsed > 0) strReturn += ", ";
                strReturn += String.Format("{0}d", Math.Abs(DayEarth));
                intUsed++;
            }

            if (intUsed < segments && (HourEarth != 0 || intUsed > 0))
            {
                if (intUsed > 0) strReturn += ", ";
                strReturn += String.Format("{0:00}", Math.Abs(HourEarth));
                intUsed++;
            }
            if (intUsed < segments && (Minute != 0 || intUsed > 0))
            {
                if (intUsed > 0) strReturn += ":";
                strReturn += String.Format("{0:00}", Math.Abs(Minute));
                intUsed++;
            }
            if (intUsed < segments)// && (Second != 0 || intUsed > 0))
            {
                if (intUsed > 0) strReturn += ":";
                strReturn += String.Format("{0:00}", Math.Abs(Second));
                intUsed++;
            }


            return strReturn;
        }

        public String DateString()
        {
            return String.Format("Year {0},Day {1}, {2}h, {3}m, {4}s", YearEarth + 1, DayEarth + 1, HourEarth, Minute, Second);
        }

        public String DateTimeString()
        {
            return String.Format("Year {0},Day {1}, {2:00}:{3:00}:{4:00}", YearEarth + 1, DayEarth + 1, HourEarth, Minute, Second);
        }

        public String IntervalStringLong()
        {
            String strReturn = "";
            if (UT < 0) strReturn += "+ ";
            strReturn += String.Format("{0} Years, {1} Days, {2:00}:{3:00}:{4:00}", Math.Abs(YearEarth), Math.Abs(DayEarth), Math.Abs(HourEarth), Math.Abs(Minute), Math.Abs(Second));
            return strReturn;
        }

        public String UTString()
        {
            String strReturn = "";
            if (UT < 0) strReturn += "+ ";
            strReturn += String.Format("{0:N0}s", Math.Abs(UT));
            return strReturn;
        }
        #endregion

        public override String ToString()
        {
            return IntervalStringLong();
        }

#region "Static Functions"
        //fudging for dates
        public static KACTime timeDateOffest = new KACTime(1, 1, 0, 0, 0);

        public static Double BuildUTFromRaw(String Years, String Days, String Hours, String Minutes, String Seconds)
        {
            return BuildUTFromRaw(Convert.ToDouble(Years), Convert.ToDouble(Days), Convert.ToDouble(Hours), Convert.ToDouble(Minutes), Convert.ToDouble(Seconds));
        }
        public static Double BuildUTFromRaw(double Years, double Days, double Hours, double Minutes, double Seconds)
        {
         return Seconds +
            Minutes * 60 +
            Hours * 60 * 60 +
            Days * 24 * 60 * 60 +
            Years * HoursPerYearEarth * 60 * 60;
        }

        public static String PrintInterval(KACTime timeTemp,  KACTime.PrintTimeFormat TimeFormat)
        {
            return PrintInterval(timeTemp, 3, TimeFormat);
        }

        public static String PrintInterval(KACTime timeTemp, int Segments, KACTime.PrintTimeFormat TimeFormat)
        {
            switch (TimeFormat )
            {
                case PrintTimeFormat.TimeAsUT:
                    return timeTemp.UTString();
                case PrintTimeFormat.KSPString:
                    return timeTemp.IntervalString(Segments);
                case PrintTimeFormat.DateTimeString:
                    return timeTemp.IntervalDateTimeString(Segments);
                default:
                    return timeTemp.IntervalString(Segments);
            }
        }

        public static String PrintDate(KACTime timeTemp, KACTime.PrintTimeFormat TimeFormat)
        {
            switch (TimeFormat)
            {
                case PrintTimeFormat.TimeAsUT:
                    return timeTemp.UTString();
                case PrintTimeFormat.KSPString:
                    return timeTemp.DateString();
                case PrintTimeFormat.DateTimeString:
                    return timeTemp.DateTimeString();
                default:
                    return timeTemp.DateTimeString();
            }
        }

        public enum PrintTimeFormat
        {
            TimeAsUT,
            KSPString,
            DateTimeString
        }
#endregion
    }

    public class KACTimeStringArray
    {
        public enum TimeEntryPrecision
        {
            Seconds = 0,
            Minutes = 1,
            Hours = 2,
            Days = 3,
            Years = 4
        }


        private String _Years="",_Days="",_Hours="",_Minutes="",_Seconds="";

        public String Years { get { return _Years; } set { _Years = value; SetValid(); } }
        public String Days { get { return _Days; } set { _Days = value; SetValid(); } }
        public String Hours { get { return _Hours; } set { _Hours = value; SetValid(); } }
        public String Minutes { get { return _Minutes; } set { _Minutes = value; SetValid(); } }
        public String Seconds { get { return _Seconds; } set { _Seconds = value; SetValid(); } }

        public Boolean Valid { get { return _Valid; } }
        Boolean _Valid=true;

        public KACTimeStringArray() {}
        public KACTimeStringArray(Double NewUT)
        {
            BuildFromUT(NewUT);
        }

        private void SetValid()
        {
            Double dblTest;
            if (Double.TryParse(_Years, out dblTest) && 
                Double.TryParse(_Days, out dblTest) && 
                Double.TryParse(_Hours, out dblTest) && 
                Double.TryParse(_Minutes, out dblTest) && 
                Double.TryParse(_Seconds, out dblTest)
                )
            {
                _Valid = true;
            }
            else
            {
                _Valid = false;
            }
        }

        public void BuildFromUT(Double UT)
        {
            KACTime timeTemp = new KACTime(UT);
            Years = timeTemp.YearEarth.ToString();
            Days = timeTemp.DayEarth.ToString();
            Hours = timeTemp.HourEarth.ToString();
            Minutes = timeTemp.Minute.ToString();
            Seconds = timeTemp.Second.ToString();
        }

        public double UT
        {
            get 
            {
                return KACTime.BuildUTFromRaw(
                    ZeroString(Years), 
                    ZeroString(Days), 
                    ZeroString(Hours), 
                    ZeroString(Minutes), 
                    ZeroString(Seconds)
                    );
            }
        }
        private String ZeroString(String strInput)
        {
            Double dblTemp;
            if (!Double.TryParse(strInput,out dblTemp))
                return "0";
            else
                return strInput;
        }
    }

    /// <summary>
    /// Class to hold an Alarm event
    /// </summary>
    public class KACAlarm
    {
        //Some Structures
        //"R","M","A","P","A","D","S","X"
        public enum AlarmType
        {
            Raw,
            Maneuver,
            ManeuverAuto,
            Apoapsis,
            Periapsis,
            AscendingNode,
            DescendingNode,
            LaunchRendevous,
            Closest,
            SOIChange,
            SOIChangeAuto,
            Transfer,
            TransferModelled,
            Distance,
            Crew,
            EarthTime
        }


        public static Dictionary<AlarmType, int> AlarmTypeToButton = new Dictionary<AlarmType, int>() {
            {AlarmType.Raw, 0},
            {AlarmType.Maneuver , 1},
            {AlarmType.ManeuverAuto , 1},
            {AlarmType.Apoapsis , 2},
            {AlarmType.Periapsis , 2},
            {AlarmType.AscendingNode , 3},
            {AlarmType.DescendingNode , 3},
            {AlarmType.LaunchRendevous , 3},
            {AlarmType.Closest , 4},
            {AlarmType.Distance , 4},
            {AlarmType.SOIChange , 5},
            {AlarmType.SOIChangeAuto , 5},
            {AlarmType.Transfer , 6},
            {AlarmType.TransferModelled , 6},
            {AlarmType.Crew , 7}
        };
        public static Dictionary<int,AlarmType> AlarmTypeFromButton = new Dictionary<int,AlarmType>() {
            {0,AlarmType.Raw},
            {1,AlarmType.Maneuver },
            {2,AlarmType.Apoapsis },
            {3,AlarmType.AscendingNode },
            {4,AlarmType.Closest },
            {5,AlarmType.SOIChange },
            {6,AlarmType.Transfer },
            {7,AlarmType.Crew }
        };




        public enum AlarmAction
        {
            MessageOnly,
            KillWarp,
            PauseGame
        }

        public String SaveName = "";                                    //Which Save File
        public String VesselID = "";                                    //uniqueID of Vessel
        public String Name = "";                                        //Name of Alarm
        //public String Message = "";                                     //Some descriptive text
        public String Notes = "";                                       //Entered extra details
        public AlarmType TypeOfAlarm=AlarmType.Raw;                     //What Type of Alarm

        public KACTime AlarmTime = new KACTime();                       //UT of the alarm
        public Double AlarmMarginSecs = 0;                              //What the margin from the event was
        public Boolean Enabled = true;                                  //Whether it is enabled - not in use currently
        public Boolean HaltWarp = true;                                 //Whether the time warp will be halted at this event
        public Boolean PauseGame = false;                               //Whether the Game will be paused at this event

        public Double ActionedAt = 0;                                   //At What UT an alarm was actioned at (for use in not refiring fired events on ship change)

        public Boolean DeleteOnClose;                                   //Whether the checkbox is on or off for this

        //public ManeuverNode ManNode;                                  //Stored ManeuverNode attached to alarm
        public List<ManeuverNode> ManNodes=null;                        //Stored ManeuverNode's attached to alarm

        public String XferOriginBodyName = "";                          //Stored orbital transfer details
        public String XferTargetBodyName = "";

        //Have to generate these details when the target object is set
        private ITargetable _TargetObject = null;                       //Stored Target Details
        
        //Vessel Target - needs the fancy get routine as the alarms load before the vessels are loaded.
        //This means that each time the object is accessed if its not yet loaded it trys again
        public ITargetable TargetObject
        {
            get {
                if (_TargetObject != null)
                    return _TargetObject;
                else
                {
                    //is there something to load here from the string
                    if (TargetLoader != "")
                    {
                        String[] TargetParts = TargetLoader.Split(",".ToCharArray());
                        switch (TargetParts[0])
                        {
                            case "Vessel":
                                if (KACWorker.StoredVesselExists(TargetParts[1]))
                                    _TargetObject = KACWorker.StoredVessel(TargetParts[1]);
                                break;
                            case "CelestialBody":
                                if (KACWorker.CelestialBodyExists(TargetParts[1]))
                                    TargetObject = KACWorker.CelestialBody(TargetParts[1]);
                                break;
                            default:
                                KACWorker.DebugLogFormatted("No Target Found:{0}", TargetLoader);
                                break;
                        }
                    }
                    return _TargetObject;
                }
            }
            set { _TargetObject = value; }
        }
        //Need this one as some vessels arent loaded when the config comes in
        public String TargetLoader = "";

        //Dynamic props down here
        public KACTime Remaining = new KACTime();                        //UT value of how long till the alarm fires
        public Boolean WarpInfluence = false;                           //Whether the Warp setting is being influenced by this alarm

        public Boolean Triggered = false;                               //Has this alarm been triggered
        public Boolean Actioned = false;                                //Has the code actioned th alarm - ie. displayed its message

        //Details of the alarm message
        public Rect AlarmWindow;                                        
        public int AlarmWindowID=0;
        public int AlarmWindowHeight = 148;
        public Boolean AlarmWindowClosed = false;

        //Details of the alarm message
        public Boolean EditWindowOpen=false;                                        
        
        #region "Constructors"
        public KACAlarm()
        {
            //if (KACWorkerGameState.IsFlightMode)
            try { SaveName = HighLogic.CurrentGame.Title; }
            catch (Exception) { }
        }
        public KACAlarm(double UT)
        {
            //if (KACWorkerGameState.IsFlightMode)
            try { SaveName = HighLogic.CurrentGame.Title; }
            catch (Exception) { }
            AlarmTime.UT = UT;
        }

        public KACAlarm(String vID, String NewName, String NewNotes, double UT, Double Margin, AlarmType atype, Boolean NewHaltWarp, Boolean NewPause)
        {
            //if (KACWorkerGameState.IsFlightMode)
            try { SaveName = HighLogic.CurrentGame.Title; }
            catch (Exception) { }
            VesselID = vID;
            Name = NewName;
            Notes = NewNotes;
            AlarmTime.UT = UT;
            AlarmMarginSecs = Margin;
            TypeOfAlarm = atype;
            Remaining.UT = AlarmTime.UT - Planetarium.GetUniversalTime();
            HaltWarp = NewHaltWarp;
            PauseGame = NewPause;
        }

        public KACAlarm(String vID, String NewName, String NewNotes,  double UT, Double Margin, AlarmType atype, Boolean NewHaltWarp, Boolean NewPause, List<ManeuverNode> NewManeuvers)
            : this(vID, NewName, NewNotes, UT, Margin, atype, NewHaltWarp, NewPause)
        {
            //set maneuver node
            ManNodes = NewManeuvers;
        }

        public KACAlarm(String vID, String NewName, String NewNotes, double UT, Double Margin, AlarmType atype, Boolean NewHaltWarp, Boolean NewPause, KACXFerTarget NewTarget)
            : this(vID, NewName, NewNotes, UT, Margin, atype, NewHaltWarp, NewPause)
        {
            //Set target details
            XferOriginBodyName = NewTarget.Origin.bodyName;
            XferTargetBodyName = NewTarget.Target.bodyName;
        }

        public KACAlarm(String vID, String NewName, String NewNotes, double UT, Double Margin, AlarmType atype, Boolean NewHaltWarp, Boolean NewPause, ITargetable NewTarget)
            : this(vID,NewName,NewNotes,UT,Margin,atype,NewHaltWarp,NewPause)
        {
            //Set the ITargetable proerty
            TargetObject = NewTarget;
        }
        #endregion

        /// <summary>
        /// Convert properties for the save file to a single String for storage
        /// </summary>
        /// <returns>CSV String of persistant properties</returns>
        public String SerializeString()
        {
            return KACUtils.PipeSepVariables(SaveName, Name, Enabled, AlarmTime.UT, HaltWarp, PauseGame, Notes);
        }

        public String SerializeString2()
        {
            //"VesselID, Name, Notes, AlarmTime.UT, AlarmMarginSecs, Type, Enabled,  HaltWarp, PauseGame, Manuever/Xfer"
            String strReturn = "";
            strReturn += VesselID + "|";
            strReturn += KACUtils.PipeSepVariables(Name, Notes, AlarmTime.UT, AlarmMarginSecs, TypeOfAlarm, Enabled, HaltWarp, PauseGame);
            strReturn += "|";

            if (ManNodes != null)
            {
                strReturn += ManNodeSerializeList(ManNodes);
            } 
            else if (XferTargetBodyName!=null && XferTargetBodyName!="")
            {
                strReturn += "" + XferOriginBodyName;
                strReturn += "," + XferTargetBodyName;
            }
            else if (TargetObject != null)
            {
                strReturn += KACAlarm.TargetSerialize(TargetObject);
            }
            return strReturn;
        }

        //String is "VesselID|Name|Notes|AlarmTime.UT|AlarmMarginSecs|Type|Enabled|HaltWarp|PauseGame|ActionedAt|Manuever|Xfer|Target|Options|<ENDLINE>");
        public String SerializeString3()
        {
            //"VesselID, Name, Notes, AlarmTime.UT, AlarmMarginSecs, Type, Enabled,  HaltWarp, PauseGame, Manuever/Xfer"
            String strReturn = "";
            strReturn += VesselID + "|";
            strReturn += KACUtils.PipeSepVariables(Name, Notes, AlarmTime.UT, AlarmMarginSecs, TypeOfAlarm, Enabled, HaltWarp, PauseGame,ActionedAt);
            strReturn += "|";

            if (ManNodes != null)
            {
                strReturn += ManNodeSerializeList(ManNodes);
            }
            strReturn += "|";

            if (XferTargetBodyName != null && XferTargetBodyName != "")
            {
                strReturn += "" + XferOriginBodyName;
                strReturn += "," + XferTargetBodyName;
            }
            strReturn += "|";
            
            if (TargetObject != null)
            {
                strReturn += KACAlarm.TargetSerialize(TargetObject);
            }
            strReturn += "|";

            //Extra Options go here if we need it later
            strReturn += "|";
            return strReturn;
        }


         /// <summary>
        /// Basically deserializing the alarm
        /// </summary>
        /// <param name="AlarmDetails"></param>
        public void LoadFromString(String AlarmDetails)
        {
            String[] vars = AlarmDetails.Split("|".ToCharArray());
            VesselID = "";
            SaveName = vars[0];
            Name = vars[1];
            Enabled = Convert.ToBoolean(vars[2]);
            AlarmTime.UT = Convert.ToDouble(vars[3]);
            HaltWarp = Convert.ToBoolean(vars[4]);
            if (vars.Length == 6)
                Notes = vars[5];
            else
            {
                PauseGame = Convert.ToBoolean(vars[5]);
                Notes = vars[6];
            }
        }

        public void LoadFromString2(String AlarmDetails)
        {
            String[] vars = AlarmDetails.Split("|".ToCharArray(), StringSplitOptions.None);
            SaveName = HighLogic.CurrentGame.Title;
            VesselID = vars[0];
            Name = vars[1];
            Notes = vars[2];
            AlarmTime.UT = Convert.ToDouble(vars[3]);
            AlarmMarginSecs = Convert.ToDouble(vars[4]);
            TypeOfAlarm = (KACAlarm.AlarmType)Enum.Parse(typeof(KACAlarm.AlarmType), vars[5]);
            Enabled = Convert.ToBoolean(vars[6]);
            HaltWarp = Convert.ToBoolean(vars[7]);
            PauseGame = Convert.ToBoolean(vars[8]);

            String strOptions = vars[9];
            switch (TypeOfAlarm)
            {
                case AlarmType.Maneuver:
                    //Generate the Nodes List from the string
                    ManNodes = ManNodeDeserializeList(strOptions);
                    break;
                case AlarmType.Transfer:
                    try
                    {
                        String[] XferParts = strOptions.Split(",".ToCharArray());
                        XferOriginBodyName = XferParts[0];
                        XferTargetBodyName = XferParts[1];
                    }
                    catch (Exception ex)
                    {
                        KACWorker.DebugLogFormatted("Unable to load transfer details for {0}", Name);
                        KACWorker.DebugLogFormatted(ex.Message);
                    }
                    break;
                case AlarmType.AscendingNode:
                case AlarmType.DescendingNode:
                case AlarmType.LaunchRendevous:
                    if (strOptions != "")
                    {
                        //find the targetable object and set it
                        TargetObject = TargetDeserialize(strOptions);               
                        if (TargetObject==null && strOptions.StartsWith("Vessel,"))
                            TargetLoader = strOptions;
                    }
                    break;
                default:
                    break;
            }
        }

        public void LoadFromString3(String AlarmDetails,Double CurrentUT)
        {
            //String is "VesselID|Name|Notes|AlarmTime.UT|AlarmMarginSecs|Type|Enabled|HaltWarp|PauseGame|ActionedAt|Manuever|Xfer|Target|Options|<ENDLINE>");

            String[] vars = AlarmDetails.Split("|".ToCharArray(), StringSplitOptions.None);
            this.SaveName = HighLogic.CurrentGame.Title;
            this.VesselID = vars[0];
            this.Name = KACUtils.DecodeVarStrings(vars[1]);
            this.Notes = KACUtils.DecodeVarStrings(vars[2]);
            this.AlarmTime.UT = Convert.ToDouble(vars[3]);
            this.AlarmMarginSecs = Convert.ToDouble(vars[4]);
            this.TypeOfAlarm = (KACAlarm.AlarmType)Enum.Parse(typeof(KACAlarm.AlarmType), vars[5]);
            this.Enabled = Convert.ToBoolean(vars[6]);
            this.HaltWarp = Convert.ToBoolean(vars[7]);
            this.PauseGame = Convert.ToBoolean(vars[8]);
            this.ActionedAt = Convert.ToDouble(vars[9]);

            if (vars[10] != "")
                this.ManNodes = ManNodeDeserializeList(vars[10]);

            if (vars[11] != "")
            {
                try
                {
                    String[] XferParts = vars[11].Split(",".ToCharArray());
                    this.XferOriginBodyName = XferParts[0];
                    this.XferTargetBodyName = XferParts[1];
                }
                catch (Exception ex)
                {
                    KACWorker.DebugLogFormatted("Unable to load transfer details for {0}", Name);
                    KACWorker.DebugLogFormatted(ex.Message);
                }
            }
            if (vars[12] != "")
            {
                //find the targetable object and set it
                this.TargetObject = TargetDeserialize(vars[12]);
                if (this.TargetObject == null && vars[12].StartsWith("Vessel,"))
                    this.TargetLoader = vars[12];
            }

            //Now do the work to set Actioned/triggered/etc if needed
            //KACWorker.DebugLogFormatted("A:{0},T:{1:0},Act:{2:0}", this.Name, CurrentUT, this.ActionedAt);
            if (ActionedAt > 0 && CurrentUT > ActionedAt)
            {
                KACWorker.DebugLogFormatted("Suppressing Alarm on Load:{0}", this.Name);
                this.Triggered = true;
                this.Actioned = true;
                this.AlarmWindowClosed = true;
            }
            else if (ActionedAt > CurrentUT)
            {
                KACWorker.DebugLogFormatted("Reenabling Alarm on Load:{0}", this.Name);
                this.Triggered = false;
                this.Actioned = false;
                this.ActionedAt = 0;
                this.AlarmWindowClosed = false;
            }

        }


        public static ITargetable TargetDeserialize(String strInput)
        {
            ITargetable tReturn = null;
            String[] TargetParts = strInput.Split(",".ToCharArray());
            switch (TargetParts[0])
            {
                case "Vessel":
                    if (KACWorker.StoredVesselExists(TargetParts[1]))
                        tReturn = KACWorker.StoredVessel(TargetParts[1]);
                    break;
                case "CelestialBody":
                    if (KACWorker.CelestialBodyExists(TargetParts[1]))
                        tReturn = KACWorker.CelestialBody(TargetParts[1]);
                    break;
                default:
                    break;
            }
            return tReturn;
        }

        public static String TargetSerialize(ITargetable tInput)
        {
            string strReturn = "";

            strReturn += tInput.GetType();
            strReturn += ",";

            if (tInput is Vessel)
            {
                Vessel tmpVessel = tInput as Vessel;
                strReturn += tmpVessel.id.ToString();
            }
            else if (tInput is CelestialBody)
            {
                CelestialBody tmpBody = tInput as CelestialBody;
                strReturn += tmpBody.bodyName;
            }

            return strReturn;

        }

        public static List<ManeuverNode> ManNodeDeserializeList(String strInput)
        {
            List<ManeuverNode> lstReturn = new List<ManeuverNode>();

            String[] strInputParts = strInput.Split(",".ToCharArray());
            KACWorker.DebugLogFormatted("Found {0} Maneuver Nodes to deserialize", strInputParts.Length / 8);

            //There are 8 parts per mannode
            for (int iNode = 0; iNode < strInputParts.Length / 8; iNode++)
            {
                String strTempNode = String.Join(",", strInputParts.Skip(iNode * 8).Take(8).ToArray());
                lstReturn.Add(ManNodeDeserialize(strTempNode));
            }

            return lstReturn;
        }

        public static ManeuverNode ManNodeDeserialize(String strInput)
        {
            ManeuverNode mReturn =  new ManeuverNode();
            String[] manparts = strInput.Split(",".ToCharArray());
            mReturn.UT = Convert.ToDouble(manparts[0]);
            mReturn.DeltaV = new Vector3d(Convert.ToDouble(manparts[1]),
                                        Convert.ToDouble(manparts[2]),
                                        Convert.ToDouble(manparts[3])
                    );
            mReturn.nodeRotation = new Quaternion(Convert.ToSingle(manparts[4]),
                                                Convert.ToSingle(manparts[5]),
                                                Convert.ToSingle(manparts[6]),
                                                Convert.ToSingle(manparts[7])
                    );
            return mReturn;
        }

        public static string ManNodeSerializeList(List<ManeuverNode> mInput)
        {
            String strReturn = "";
            foreach (ManeuverNode tmpMNode in mInput)
            {
                strReturn += ManNodeSerialize(tmpMNode);
                strReturn += ",";
            }
            strReturn = strReturn.TrimEnd(",".ToCharArray());
            return strReturn;
        }

        public static string ManNodeSerialize(ManeuverNode mInput)
        {
            String strReturn = mInput.UT.ToString();
            strReturn += "," + KACUtils.CommaSepVariables(mInput.DeltaV.x, mInput.DeltaV.y, mInput.DeltaV.z);
            strReturn += "," + KACUtils.CommaSepVariables(mInput.nodeRotation.x, mInput.nodeRotation.y, mInput.nodeRotation.z, mInput.nodeRotation.w);
            return strReturn;
        }

        public static Boolean CompareManNodeListSimple(List<ManeuverNode> l1, List<ManeuverNode> l2)
        {
            Boolean blnReturn = true;

            if (l1.Count != l2.Count)
                blnReturn=false;
            else
            {
                for (int i = 0; i < l1.Count; i++)
                {
                    if (l1[i].UT != l2[i].UT)
                        blnReturn = false;
                    else if (l1[i].DeltaV != l2[i].DeltaV)
                        blnReturn = false;
                }
            }

            return blnReturn;
        }



        public static int SortByUT(KACAlarm c1, KACAlarm c2)
        {
            return c1.Remaining.UT.CompareTo(c2.Remaining.UT);
        }
    }

    /// <summary>
    /// Extended List class to deal with multiple save files"/>
    /// </summary>
    public class KACAlarmList : List<KACAlarm>
    {
        /// <summary>
        /// How many alarms in the supplied save file
        /// </summary>
        /// <param name="SaveName"></param>
        /// <returns></returns>
        public Int64 CountInSave(String SaveName)
        {
            long lngReturn=0;

            foreach (KACAlarm tmpAlarm in this)
            {
                if (tmpAlarm.SaveName.ToLower() == SaveName.ToLower())
                    lngReturn++;
            }

            return lngReturn;
        }

        /// <summary>
        /// Are there any alarms for this save file that are in the future and not already actioned
        /// </summary>
        /// <param name="SaveName"></param>
        /// <returns></returns>
        public Boolean ActiveEnabledFutureAlarms(String SaveName)
        {
            Boolean blnReturn = false;
            foreach (KACAlarm tmpAlarm in this)
            {
                if (tmpAlarm.AlarmTime.UT > Planetarium.GetUniversalTime() && tmpAlarm.Enabled && !tmpAlarm.Actioned && (tmpAlarm.SaveName.ToLower() == SaveName.ToLower()))
                {
                    blnReturn = true;
                }
            }
            return blnReturn;
        }

        /// <summary>
        /// Get a filtered list of alarms for a specirfic save file
        /// </summary>
        /// <param name="SaveName"></param>
        /// <returns></returns>
        public KACAlarmList BySaveName(String SaveName)
        {
            KACAlarmList lstreturn = new KACAlarmList();

            foreach (KACAlarm tmpAlarm in this)
            {
                if (tmpAlarm.SaveName.ToLower() == SaveName.ToLower())
                    lstreturn.Add(tmpAlarm);
            }

            return lstreturn;
        }

        /// <summary>
        /// Get the Alarm object from the Unity Window ID
        /// </summary>
        /// <param name="windowID"></param>
        /// <returns></returns>
        public KACAlarm GetByWindowID(Int32 windowID)
        {
            KACAlarm alarmReturn=null;
            foreach (KACAlarm tmpAlarm in this)
            {
                if (tmpAlarm.AlarmWindowID == windowID)
                    alarmReturn=tmpAlarm;
            }
            return alarmReturn;
        }

        public Boolean PauseAlarmOnScreen(String SaveName)
        {
            Boolean blnReturn = false;
            foreach (KACAlarm tmpAlarm in this)
            {
                if ((tmpAlarm.SaveName.ToLower() == SaveName.ToLower()) && tmpAlarm.AlarmWindowID!=0 && !tmpAlarm.AlarmWindowClosed )
                {
                    blnReturn = true;
                    break;
                }
            }
            return blnReturn;
        }
    }

    public class KACVesselSOI
    {
        public String Name;
        public String SOIName;
        //public String SOINew;
        //public Boolean SOIChanged { get { return (SOILast != SOINew); } }

        //public KACVesselSOI() { }
        public KACVesselSOI(String VesselName, String SOIBody)
        {
            Name = VesselName;
            SOIName = SOIBody;
        }
    }

    public class KACXFerTarget
        {
            private CelestialBody _Origin;

            public CelestialBody Origin
            {
                get { return _Origin; }
                set { 
                    _Origin = value;
                    if (_Target != null)
                    {
                        CalcPhaseAngleTarget();
                        CalcPhaseAngleCurrent();
                    }
                }
            }
            private CelestialBody _Target;

            public CelestialBody Target
            {
                get { return _Target; }
                set { 
                    _Target = value;
                    if (_Target != null)
                    {
                        CalcPhaseAngleTarget();
                        CalcPhaseAngleCurrent();
                    }
                }
            }
            

            private double _PhaseAngleTarget;
            private double _PhaseAngleCurrent;
            public double PhaseAngleTarget
            {
                get
                {
                    CalcPhaseAngleTarget();
                    return KACUtils.clampDegrees(_PhaseAngleTarget); }
                }

            private void CalcPhaseAngleTarget()
            {
                _PhaseAngleTarget = KACUtils.clampDegrees360(180 * (1 - Math.Pow((Origin.orbit.semiMajorAxis + Target.orbit.semiMajorAxis) / (2 * Target.orbit.semiMajorAxis), 1.5)));
            }
            public double PhaseAngleCurrent
            {
                get
                {
                    CalcPhaseAngleCurrent();
                    return KACUtils.clampDegrees(_PhaseAngleCurrent);
                }
            }

            private void CalcPhaseAngleCurrent()
            {
                _PhaseAngleCurrent = KACUtils.clampDegrees360(Target.orbit.trueAnomaly + Target.orbit.argumentOfPeriapsis + Target.orbit.LAN
                    - (Origin.orbit.trueAnomaly + Origin.orbit.argumentOfPeriapsis + Origin.orbit.LAN));
            }

            public double PhaseAngleTarget360 {get{return KACUtils.clampDegrees360(_PhaseAngleTarget); }}
            public double PhaseAngleCurrent360 {get{return KACUtils.clampDegrees360(_PhaseAngleCurrent); }}

            private KACTime _AlignmentTime= new KACTime();
            public KACTime AlignmentTime
            {
                get
                {
                    double angleChangepersec = (360 / Target.orbit.period) - (360 / Origin.orbit.period);
                    double angleToMakeUp =PhaseAngleCurrent360-PhaseAngleTarget360;
                    if (angleToMakeUp > 0 && angleChangepersec > 0)
                        angleToMakeUp -= 360;
                    if (angleToMakeUp < 0 && angleChangepersec < 0)
                        angleToMakeUp += 360;

                    double UTToTarget = Math.Floor(Math.Abs(angleToMakeUp / angleChangepersec));
                    KACTime tmeReturn = new KACTime(UTToTarget);
                    return tmeReturn;
                }
            }

        }

    public class KACXFerModelPoint
    {
        public Double UT;
        public Int32 Origin;
        public Int32 Target;
        public Double PhaseAngle;

        public KACXFerModelPoint(Double NewUT, Int32 NewOrigin, Int32 NewTarget, Double NewPhase)
        {
            UT = NewUT;
            PhaseAngle = NewPhase;
            Origin = NewOrigin;
            Target = NewTarget;
        } 
    }

}
