#region

using System;
using System.Collections.Generic;

using TCompiler.Types;
using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilerTypes;
using TCompiler.Types.CompilingTypes;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion


namespace TCompiler.Settings
{
    /// <summary>
    ///     Properties/Settings
    /// </summary>
    public static class GlobalProperties
    {
        /// <summary>
        ///     The name of the external interrupt 0 execution method
        /// </summary>
        public const string EXTERNAL_INTERRUPT0_EXECUTION_NAME = "ISRE0";

        /// <summary>
        ///     The name of the external interrupt 1 execution method
        /// </summary>
        public const string EXTERNAL_INTERRUPT1_EXECUTION_NAME = "ISRE1";

        /// <summary>
        ///     The name of the timer/counter interrupt 0 execution mode
        /// </summary>
        public const string TIMER_COUNTER_INTERRUPT0_EXECUTION_NAME = "ISRT0";

        /// <summary>
        ///     The name of the timer/counter interrupt 1 execution mode
        /// </summary>
        public const string TIMER_COUNTER_INTERRUPT1_EXECUTION_NAME = "ISRT1";

        /// <summary>
        ///     The limit of the internal RAM Byte variable index
        /// </summary>
        public const int INTERNAL_MEMORY_BYTE_VARIABLE_LIMIT = 0x80;

        /// <summary>
        ///     The limit of the internal RAM Bit variable index
        /// </summary>
        public const int INTERNAL_MEMORY_BIT_VARIABLE_LIMIT = 0x2F;

        /// <summary>
        ///     A list of invalid names (for variables and methods)
        /// </summary>
        public static readonly List <string> InvalidNames = new List <string>
        {
            //"add",
            //"subb",
            //"inc",
            //"dec",
            //"mul",
            //"div",
            //"da",
            //"anl",
            //"orl",
            //"xrl",
            //"clr",
            //"cpl",
            //"mov",
            //"movc",
            //"movx",
            //"push",
            //"pop",
            //"xch",
            //"xchd",
            //"swap",
            //"nop",
            //"setb",
            //"rl",
            //"rlc",
            //"rr",
            //"rrc",
            //"call",
            //"ret",
            //"jmp",
            //"jz",
            //"jnz",
            //"jc",
            //"jnc",
            //"jb",
            //"jnb",
            //"jbc",
            //"cjne",
            //"djnz",
            //"data",
            //"bit",
            //"include",
            "int",
            "if",
            "endif",
            "bool",
            "while",
            "endwhile",
            "break",
            "block",
            "endblock",
            "fortil",
            "endfortil",
            "cint",
            "char",
            "return",
            "method",
            "endmethod",
            "sleep",
            "isrexternal0",
            "isrexternal1",
            "isrtimer0",
            "isrtimer1",
            "isrcounter0",
            "isrcounter1",
            "endisr",
            "include"
        };

        /// <summary>
        ///     A list of the variables of the Special Function Register that can be used in T with their equivalent names
        /// </summary>
        public static readonly List <Variable> StandardVariables = new List <Variable>
        {
            new Int (new Address (0x080, false), "Port0", false),        //p0
            new Int (new Address (0x081, false), "StackPointer", false), //sp
            //new Int(false, "082h", "DataPointerLow"),                                             //dpl 
            //new Int(false, "083h", "DataPointerHigh"),                                            //dph
            new Int (new Address (0x082, false), "DataPointer0Low", false),                //dp0l
            new Int (new Address (0x083, false), "DataPointer0High", false),               //dp0h
            new Int (new Address (0x084, false), "DataPointer1Low", false),                //dp1l
            new Int (new Address (0x085, false), "DataPointer1High", false),               //dp1h
            new Int (new Address (0x086, false), "SPDR_Register", false),                  //spdr
            new Int (new Address (0x087, false), "PowerControl", false),                   //pcon
            new Int (new Address (0x088, false), "TimerControl", false),                   //tcon
            new Int (new Address (0x089, false), "TimerMode", false),                      //tmod
            new Int (new Address (0x08A, false), "Timer0Low", false),                      //tl0
            new Int (new Address (0x08B, false), "Timer1Low", false),                      //tl1
            new Int (new Address (0x08C, false), "Timer0High", false),                     //th0
            new Int (new Address (0x08D, false), "Timer1High", false),                     //th1
            new Int (new Address (0x090, false), "Port1", false),                          //p1
            new Int (new Address (0x096, false), "Watchdog_MemoryControlRegister", false), //wmcon
            new Int (new Address (0x098, false), "SerialControl", false),                  //scon
            new Int (new Address (0x099, false), "SerialBuffer", false),                   //sbuf
            new Int (new Address (0x0A0, false), "Port2", false),                          //p2
            new Int (new Address (0x0A8, false), "InterruptEnable", false),                //ie
            new Int (new Address (0x0AA, false), "SPSR_Register", false),                  //spsr
            new Int (new Address (0x0B0, false), "Port3", false),                          //p3
            new Int (new Address (0x0B8, false), "InterruptPriority", false),              //ip
            new Int (new Address (0x0C8, false), "Timer2Control", false),                  //t2con
            new Int (new Address (0x0C9, false), "Timer2Mode", false),                     //t2mod
            new Int (new Address (0x0CA, false), "CaptureRegisterLow", false),             //rcap2l
            new Int (new Address (0x0CC, false), "CaptureRegisterHigh", false),            //rcap2h
            new Int (new Address (0x0CC, false), "Timer2Low", false),                      //tl2
            new Int (new Address (0x0CD, false), "Timer2High", false),                     //th2
            new Int (new Address (0x0D0, false), "ProgramStatusWorld", false),             //psw
            new Int (new Address (0x0D5, false), "SPCR_Register", false),                  //spcr
            new Int (new Address (0x0E0, false), "Accumulator", false),                    //acc
            new Int (new Address (0x0F0, false), "BRegister", false),                      //b
            //new Int(false, "0E0h", "a"),                                                          //a
            //ProgramStatusWorld
            new Bool (new Address (0x0D0, false, 7), "Carry", false),                     //cy
            new Bool (new Address (0x0D0, false, 6), "AuxiliaryCarry", false),            //ac
            new Bool (new Address (0x0D0, false, 5), "GeneralPurposeStatusFlag0", false), //f0
            new Bool (new Address (0x0D0, false, 4), "RegisterBankSelectBit1", false),    //rs1
            new Bool (new Address (0x0D0, false, 3), "RegisterBankSelectBit0", false),    //rs0
            new Bool (new Address (0x0D0, false, 2), "OverflowFlag", false),              //ov
            new Bool (new Address (0x0D0, false, 1), "GeneralPurposeStatusFlag1", false), //f1
            new Bool (new Address (0x0D0, false, 0), "ParityFlag", false),                //p
            //TimerControl      
            new Bool (new Address (0x088, false, 7), "Timer1Overflow", false),       //tf1
            new Bool (new Address (0x088, false, 6), "Timer1Run", false),            //tr1
            new Bool (new Address (0x088, false, 5), "Timer0Overflow", false),       //tf0
            new Bool (new Address (0x088, false, 4), "Timer0Run", false),            //tr0
            new Bool (new Address (0x088, false, 3), "Interrupt1EdgeFlag", false),   //ie1
            new Bool (new Address (0x088, false, 2), "Interrupt1SignalType", false), //it1
            new Bool (new Address (0x088, false, 1), "Interrupt0EdgeFlag", false),   //ie0
            new Bool (new Address (0x088, false, 0), "Interrupt0SignalType", false), //it0
            //InterruptEnable       
            new Bool (new Address (0x0A8, false, 7), "EnableAllInterrupts", false),       //ea
            new Bool (new Address (0x0A8, false, 5), "EnableTimer2Interrupt", false),     //et2
            new Bool (new Address (0x0A8, false, 4), "EnableSerialPortInterrupt", false), //es
            new Bool (new Address (0x0A8, false, 3), "EnableTimer1Interrupt", false),     //et1
            new Bool (new Address (0x0A8, false, 2), "EnableExternalInterrupt1", false),  //ex1
            new Bool (new Address (0x0A8, false, 1), "EnableTimerInterrupt0", false),     //et0
            new Bool (new Address (0x0A8, false, 0), "EnableExternalInterrupt0", false),  //ex0
            //InterruptPriority    
            new Bool (new Address (0x0B8, false, 5), "Timer2InterruptPriority", false),     //pt2
            new Bool (new Address (0x0B8, false, 4), "SerialPortInterruptPriority", false), //ps
            new Bool (new Address (0x0B8, false, 3), "Timer1InterruptPriority", false),     //pt1
            new Bool (new Address (0x0B8, false, 2), "External1InterruptPriority", false),  //px1
            new Bool (new Address (0x0B8, false, 1), "Timer0InterruptPriority", false),     //pt0
            new Bool (new Address (0x0B8, false, 0), "External0InterruptPriority", false),  //px0
            //Port3
            new Bool (new Address (0x0B0, false, 7), "DataMemoryRead", false),      //rd
            new Bool (new Address (0x0B0, false, 6), "DataMemoryWrite", false),     //wr
            new Bool (new Address (0x0B0, false, 5), "Timer1ExternalInput", false), //t1
            new Bool (new Address (0x0B0, false, 4), "Timer0ExternalInput", false), //t0
            new Bool (new Address (0x0B0, false, 3), "Interrupt1", false),          //int1
            new Bool (new Address (0x0B0, false, 2), "Interrupt0", false),          //int0
            new Bool (new Address (0x0B0, false, 1), "SerialOutputPort", false),    //txd
            new Bool (new Address (0x0B0, false, 0), "SerialInputPort", false),     //rxd
            //Timer2Counter         
            new Bool (new Address (0x0C8, false, 7), "Timer2Overflow", false),        //tf2
            new Bool (new Address (0x0C8, false, 6), "Timer2ExternalFlag", false),    //exf2
            new Bool (new Address (0x0C8, false, 5), "ReceiveClockEnable", false),    //rclk
            new Bool (new Address (0x0C8, false, 4), "TransmitClockEnable", false),   //tclk
            new Bool (new Address (0x0C8, false, 3), "Timer2ExternalEnable", false),  //exen2
            new Bool (new Address (0x0C8, false, 2), "Timer2Run", false),             //tr2
            new Bool (new Address (0x0C8, false, 1), "Counter_Timer2Select", false),  //c_t2
            new Bool (new Address (0x0C8, false, 0), "Capture_Reload2Select", false), //cp_rl2
            //SerialCounter
            new Bool (new Address (0x098, false, 7), "SerialPortMode0", false),                    //sm0
            new Bool (new Address (0x098, false, 6), "SerialPortMode1", false),                    //sm1
            new Bool (new Address (0x098, false, 5), "MultiprocessorCommunicationsEnable", false), //sm2
            new Bool (new Address (0x098, false, 4), "ReceiverEnable", false),                     //ren
            new Bool (new Address (0x098, false, 3), "TransmitBit8", false),                       //tb8
            new Bool (new Address (0x098, false, 2), "ReceiveBit8", false),                        //rb8
            new Bool (new Address (0x098, false, 1), "TransmitFlag", false),                       //ti
            new Bool (new Address (0x098, false, 0), "ReceiveFlag", false),                        //ri
            //Port1
            new Bool (new Address (0x090, false, 7), "ClockInput_Output", false),    //sck
            new Bool (new Address (0x090, false, 6), "DataInput_Output", false),     //miso
            new Bool (new Address (0x090, false, 5), "DataOutput_Input", false),     //mosi
            new Bool (new Address (0x090, false, 4), "SlavePortSelectInput", false), //ss
            new Bool (new Address (0x090, false, 1), "Timer_Counter2Capture_ReloadTrigger_DirectionControl", false),
            //t2ex
            new Bool (new Address (0x090, false, 0), "CountInputTimer_Counter2", false) //t2
            //c
        };

        /// <summary>
        ///     A list of the operations and their execution priority
        /// </summary>
        public static readonly List <OperationPriority> OperationPriorities = new List <OperationPriority>
            //todo implement priorities
            {
                new OperationPriority ("&", 0, new Tuple <bool, bool> (true, true)),
                new OperationPriority ("|", 0, new Tuple <bool, bool> (true, true)),
                new OperationPriority ("=", 1, new Tuple <bool, bool> (true, true)),
                new OperationPriority ("!=", 1, new Tuple <bool, bool> (true, true)),
                new OperationPriority (">", 1, new Tuple <bool, bool> (true, true)),
                new OperationPriority ("<", 1, new Tuple <bool, bool> (true, true)),
                new OperationPriority ("<<", 2, new Tuple <bool, bool> (true, true)),
                new OperationPriority (">>", 2, new Tuple <bool, bool> (true, true)),
                new OperationPriority ("++", 2, new Tuple <bool, bool> (true, false)),
                new OperationPriority ("--", 2, new Tuple <bool, bool> (true, false)),
                new OperationPriority ("+", 3, new Tuple <bool, bool> (true, true)),
                new OperationPriority ("-", 3, new Tuple <bool, bool> (true, true)),
                new OperationPriority ("*", 4, new Tuple <bool, bool> (true, true)),
                new OperationPriority ("/", 4, new Tuple <bool, bool> (true, true)),
                new OperationPriority ("%", 4, new Tuple <bool, bool> (true, true)),
                new OperationPriority ("!", 5, new Tuple <bool, bool> (false, true)),
                new OperationPriority (":", 6, new Tuple <bool, bool> (true, true)),
                new OperationPriority (".", 6, new Tuple <bool, bool> (true, true))
            };

        /// <summary>
        ///     A list of the assignment signs that can be used
        /// </summary>
        public static readonly List <string> AssignmentSigns = new List <string>
        {
            ":=",
            "+=",
            "-=",
            "/=",
            "*=",
            "%=",
            "|=",
            "&="
        };

        /// <summary>
        ///     The path the TCode gets passed to the compiler
        /// </summary>
        /// <value>The path as a string</value>
        public static List <string> InputPaths { get; set; }

        /// <summary>
        ///     The path the compiled assembler code gets saved to
        /// </summary>
        /// <value>The path as a string</value>
        public static string OutputPath { get; set; }

        /// <summary>
        ///     The path the errors get saved to
        /// </summary>
        /// <value>The path as a string</value>
        public static string ErrorPath { get; set; }

        public static CodeLine CurrentLine { get; set; }

        /// <summary>
        ///     The count of the current label
        /// </summary>
        /// <example>325</example>
        public static int LabelCount { private get; set; }

        /// <summary>
        ///     The current label
        /// </summary>
        /// <remarks>
        ///     At each view the labelCount is increased
        /// </remarks>
        /// <example>
        ///     The label name: L325
        /// </example>
        /// <value>The label as a Label</value>
        public static Label Label
        {
            get
            {
                LabelCount++;
                return new Label ($"l{LabelCount}", CurrentLine);
            }
        }

        /// <summary>
        ///     The current register address
        /// </summary>
        /// <remarks>It must increase/decrease</remarks>
        public static int CurrentRegisterAddress { set; get; }

        /// <summary>
        ///     The current register name
        /// </summary>
        /// <remarks>
        ///     Increases the current register address
        /// </remarks>
        /// <exception cref="TooManyRegistersException">Gets thrown when all registers are used</exception>
        public static string CurrentRegister
        {
            get
            {
                CurrentRegisterAddress++;
                if (CurrentRegisterAddress > 9)
                    throw new TooManyRegistersException (CurrentLine);
                return $"R{CurrentRegisterAddress}";
            }
        }
    }
}