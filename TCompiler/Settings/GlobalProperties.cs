#region

#region

using System.Collections.Generic;
using TCompiler.Types;
using TCompiler.Types.CompilingTypes;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation.Compare;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

// ReSharper disable CommentTypo

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
        public const string ExternalInterrupt0ExecutionName = "ISRE0";

        /// <summary>
        ///     The name of the external interrupt 1 execution method
        /// </summary>
        public const string ExternalInterrupt1ExecutionName = "ISRE1";

        public const string TimerCounterInterrupt0ExecutionName = "ISRT0";

        public const string TimerCounterInterrupt1ExecutionName = "ISRT1";

        /// <summary>
        ///     A list of invalid names (for variables and methods)
        /// </summary>
        public static readonly List<string> InvalidNames = new List<string>
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
            "endisr"
        };

        /// <summary>
        ///     A list of the variables of the Special Function Register that can be used in T with their equivalent names
        /// </summary>
        public static readonly List<Variable> StandardVariables = new List<Variable>
        {
            new Int(new Address(0x080), "Port0", false), //p0
            new Int(new Address(0x081), "StackPointer", false), //sp
            //new Int(false, "082h", "DataPointerLow"),                                             //dpl 
            //new Int(false, "083h", "DataPointerHigh"),                                            //dph
            new Int(new Address(0x082), "DataPointer0Low", false), //dp0l
            new Int(new Address(0x083), "DataPointer0High", false), //dp0h
            new Int(new Address(0x084), "DataPointer1Low", false), //dp1l
            new Int(new Address(0x085), "DataPointer1High", false), //dp1h
            new Int(new Address(0x086), "SPDR_Register", false), //spdr
            new Int(new Address(0x087), "PowerControl", false), //pcon
            new Int(new Address(0x088), "TimerControl", false), //tcon
            new Int(new Address(0x089), "TimerMode", false), //tmod
            new Int(new Address(0x08A), "Timer0Low", false), //tl0
            new Int(new Address(0x08B), "Timer1Low", false), //tl1
            new Int(new Address(0x08C), "Timer0High", false), //th0
            new Int(new Address(0x08D), "Timer1High", false), //th1
            new Int(new Address(0x090), "Port1", false), //p1
            new Int(new Address(0x096), "Watchdog_MemoryControlRegister", false), //wmcon
            new Int(new Address(0x098), "SerialControl", false), //scon
            new Int(new Address(0x099), "SerialBuffer", false), //sbuf
            new Int(new Address(0x0A0), "Port2", false), //p2
            new Int(new Address(0x0A8), "InterruptEnable", false), //ie
            new Int(new Address(0x0AA), "SPSR_Register", false), //spsr
            new Int(new Address(0x0B0), "Port3", false), //p3
            new Int(new Address(0x0B8), "InterruptPriority", false), //ip
            new Int(new Address(0x0C8), "Timer2Control", false), //t2con
            new Int(new Address(0x0C9), "Timer2Mode", false), //t2mod
            new Int(new Address(0x0CA), "CaptureRegisterLow", false), //rcap2l
            new Int(new Address(0x0CC), "CaptureRegisterHigh", false), //rcap2h
            new Int(new Address(0x0CC), "Timer2Low", false), //tl2
            new Int(new Address(0x0CD), "Timer2High", false), //th2
            new Int(new Address(0x0D0), "ProgramStatusWorld", false), //psw
            new Int(new Address(0x0D5), "SPCR_Register", false), //spcr
            new Int(new Address(0x0E0), "Accumulator", false), //acc
            new Int(new Address(0x0F0), "BRegister", false), //b
            //new Int(false, "0E0h", "a"),                                                          //a
            //ProgramStatusWorld
            new Bool(new Address(0x0D0,7), "Carry", false), //cy
            new Bool(new Address(0x0D0,6), "AuxiliaryCarry", false), //ac
            new Bool(new Address(0x0D0,5), "GeneralPurposeStatusFlag0", false), //f0
            new Bool(new Address(0x0D0,4), "RegisterBankSelectBit1", false), //rs1
            new Bool(new Address(0x0D0,3), "RegisterBankSelectBit0", false), //rs0
            new Bool(new Address(0x0D0,2), "OverflowFlag", false), //ov
            new Bool(new Address(0x0D0,1), "GeneralPurposeStatusFlag1", false), //f1
            new Bool(new Address(0x0D0,0), "ParityFlag", false), //p
            //TimerControl
            new Bool(new Address(0x088,7), "Timer1Overflow", false), //tf1
            new Bool(new Address(0x088,6), "Timer1Run", false), //tr1
            new Bool(new Address(0x088,5), "Timer0Overflow", false), //tf0
            new Bool(new Address(0x088,4), "Timer0Run", false), //tr0
            new Bool(new Address(0x088,3), "Interrupt1EdgeFlag", false), //ie1
            new Bool(new Address(0x088,2), "Interrupt1SignalType", false), //it1
            new Bool(new Address(0x088,1), "Interrupt0EdgeFlag", false), //ie0
            new Bool(new Address(0x088,0), "Interrupt0SignalType", false), //it0
            //InterruptEnable
            new Bool(new Address(0x0A8,7), "EnableAllInterrupts", false), //ea
            new Bool(new Address(0x0A8,5), "EnableTimer2Interrupt", false), //et2
            new Bool(new Address(0x0A8,4), "EnableSerialPortInterrupt", false), //es
            new Bool(new Address(0x0A8,3), "EnableTimer1Interrupt", false), //et1
            new Bool(new Address(0x0A8,2), "EnableExternalInterrupt1", false), //ex1
            new Bool(new Address(0x0A8,1), "EnableTimerInterrupt0", false), //et0
            new Bool(new Address(0x0A8,0), "EnableExternalInterrupt0", false), //ex0
            //InterruptPriority
            new Bool(new Address(0x0B8,5), "Timer2InterruptPriority", false), //pt2
            new Bool(new Address(0x0B8,4), "SerialPortInterruptPriority", false), //ps
            new Bool(new Address(0x0B8,3), "Timer1InterruptPriority", false), //pt1
            new Bool(new Address(0x0B8,2), "External1InterruptPriority", false), //px1
            new Bool(new Address(0x0B8,1), "Timer0InterruptPriority", false), //pt0
            new Bool(new Address(0x0B8,0), "External0InterruptPriority", false), //px0
            //Port3
            new Bool(new Address(0x0B0,7), "DataMemoryRead", false), //rd
            new Bool(new Address(0x0B0,6), "DataMemoryWrite", false), //wr
            new Bool(new Address(0x0B0,5), "Timer1ExternalInput", false), //t1
            new Bool(new Address(0x0B0,4), "Timer0ExternalInput", false), //t0
            new Bool(new Address(0x0B0,3), "Interrupt1", false), //int1
            new Bool(new Address(0x0B0,2), "Interrupt0", false), //int0
            new Bool(new Address(0x0B0,1), "SerialOutputPort", false), //txd
            new Bool(new Address(0x0B0,0), "SerialInputPort", false), //rxd
            //Timer2Counter
            new Bool(new Address(0x0C8,7), "Timer2Overflow", false), //tf2
            new Bool(new Address(0x0C8,6), "Timer2ExternalFlag", false), //exf2
            new Bool(new Address(0x0C8,5), "ReceiveClockEnable", false), //rclk
            new Bool(new Address(0x0C8,4), "TransmitClockEnable", false), //tclk
            new Bool(new Address(0x0C8,3), "Timer2ExternalEnable", false), //exen2
            new Bool(new Address(0x0C8,2), "Timer2Run", false), //tr2
            new Bool(new Address(0x0C8,1), "Counter_Timer2Select", false), //c_t2
            new Bool(new Address(0x0C8,0), "Capture_Reload2Select", false), //cp_rl2
            //SerialCounter
            new Bool(new Address(0x098,7), "SerialPortMode0", false), //sm0
            new Bool(new Address(0x098,6), "SerialPortMode1", false), //sm1
            new Bool(new Address(0x098,5), "MultiprocessorCommunicationsEnable", false), //sm2
            new Bool(new Address(0x098,4), "ReceiverEnable", false), //ren
            new Bool(new Address(0x098,3), "TransmitBit8", false), //tb8
            new Bool(new Address(0x098,2), "ReceiveBit8", false), //rb8
            new Bool(new Address(0x098,1), "TransmitFlag", false), //ti
            new Bool(new Address(0x098,0), "ReceiveFlag", false), //ri
            //Port1
            new Bool(new Address(0x090,7), "ClockInput_Output", false), //sck
            new Bool(new Address(0x090,6), "DataInput_Output", false), //miso
            new Bool(new Address(0x090,5), "DataOutput_Input", false), //mosi
            new Bool(new Address(0x090,4), "SlavePortSelectInput", false), //ss
            new Bool(new Address(0x090,1), "Timer_Counter2Capture_ReloadTrigger_DirectionControl", false), //t2ex
            new Bool(new Address(0x090,0), "CountInputTimer_Counter2", false) //t2
            //c
        };

        /// <summary>
        ///     The path the TCode gets passed to the compiler
        /// </summary>
        /// <value>The path as a string</value>
        public static string InputPath { get; set; }

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

        public static readonly List<OperationPriority> OperationPriorities = new List<OperationPriority>
        {
            new OperationPriority("&", typeof(And), 0),
            new OperationPriority("|", typeof(Or), 0),
            new OperationPriority("=", typeof(Equal), 1),
            new OperationPriority(">", typeof(Bigger), 1),
            new OperationPriority("<", typeof(Smaller),1),
            new OperationPriority("<<", typeof(ShiftLeft), 2),
            new OperationPriority(">>", typeof(ShiftRight), 2),
            new OperationPriority("+", typeof(Add), 3),
            new OperationPriority("-", typeof(Subtract), 3),
            new OperationPriority("*", typeof(Multiply), 4),
            new OperationPriority("/", typeof(Divide), 4),
            new OperationPriority("%", typeof(Modulo), 4),
            new OperationPriority("!", typeof(Not), 5),
            new OperationPriority(":", typeof(VariableOfCollection), 6),
            new OperationPriority(".", typeof(BitOf), 6)
        };
    }
}