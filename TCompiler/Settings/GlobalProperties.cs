#region

using System.Collections.Generic;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;
// ReSharper disable CommentTypo

#endregion

namespace TCompiler.Settings
{
    /// <summary>
    /// Properties/Settings
    /// </summary>
    public static class GlobalProperties
    {
        /// <summary>
        /// A list of invalid names (for variables and methods)
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
            "isrexternal1"
        };

        /// <summary>
        /// A list of the variables of the Special Function Register that can be used in T with their equivalent names
        /// </summary>
        public static readonly List<Variable> StandardVariables = new List<Variable>
        {
            new Int("080h", "Port0", false),                                                        //p0
            new Int("081h", "StackPointer", false),                                                 //sp
            //new Int(false, "082h", "DataPointerLow"),                                             //dpl 
            //new Int(false, "083h", "DataPointerHigh"),                                            //dph
            new Int("082h", "DataPointer0Low", false),                                              //dp0l
            new Int("083h", "DataPointer0High", false),                                             //dp0h
            new Int("084h", "DataPointer1Low", false),                                              //dp1l
            new Int("085h", "DataPointer1High", false),                                             //dp1h
            new Int("086h", "SPDR_Register", false),                                                //spdr
            new Int("087h", "PowerControl", false),                                                 //pcon
            new Int("088h", "TimerControl", false),                                                 //tcon
            new Int("089h", "TimerMode", false),                                                    //tmod
            new Int("08Ah", "Timer0Low", false),                                                    //tl0
            new Int("08Bh", "Timer1Low", false),                                                    //tl1
            new Int("08Ch", "Timer0High", false),                                                   //th0
            new Int("08Dh", "Timer1High", false),                                                   //th1
            new Int("090h", "Port1", false),                                                        //p1
            new Int("096h", "Watchdog_MemoryControlRegister", false),                               //wmcon
            new Int("098h", "SerialControl", false),                                                //scon
            new Int("099h", "SerialBuffer", false),                                                 //sbuf
            new Int("0A0h", "Port2", false),                                                        //p2
            new Int("0A8h", "InterruptEnable", false),                                              //ie
            new Int("0AAh", "SPSR_Register", false),                                                //spsr
            new Int("0B0h", "Port3", false),                                                        //p3
            new Int("0B8h", "InterruptPriority", false),                                            //ip
            new Int("0C8h", "Timer2Control", false),                                                //t2con
            new Int("0C9h", "Timer2Mode", false),                                                   //t2mod
            new Int("0CAh", "CaptureRegisterLow", false),                                           //rcap2l
            new Int("0CCh", "CaptureRegisterHigh", false),                                          //rcap2h
            new Int("0CCh", "Timer2Low", false),                                                    //tl2
            new Int("0CDh", "Timer2High", false),                                                   //th2
            new Int("0D0h", "ProgramStatusWorld", false),                                           //psw
            new Int("0D5h", "SPCR_Register", false),                                                //spcr
            new Int("0E0h", "Accumulator", false),                                                  //acc
            new Int("0F0h", "BRegister", false),                                                    //b
            //new Int(false, "0E0h", "a"),                                                          //a
                //ProgramStatusWorld
            new Bool("0D0h.7", "Carry", false),                                                     //cy
            new Bool("0D0h.6", "AuxiliaryCarry", false),                                            //ac
            new Bool("0D0h.5", "GeneralPurposeStatusFlag0", false),                                 //f0
            new Bool("0D0h.4", "RegisterBankSelectBit1", false),                                    //rs1
            new Bool("0D0h.3", "RegisterBankSelectBit0", false),                                    //rs0
            new Bool("0D0h.2", "OverflowFlag", false),                                              //ov
            new Bool("0D0h.1", "GeneralPurposeStatusFlag1", false),                                 //f1
            new Bool("0D0h.0", "ParityFlag", false),                                                //p
                //TimerControl
            new Bool("088h.7", "Timer1Overflow", false),                                            //tf1
            new Bool("088h.6", "Timer1Run", false),                                                 //tr1
            new Bool("088h.5", "Timer0Overflow", false),                                            //tf0
            new Bool("088h.4", "Timer0Run", false),                                                 //tr0
            new Bool("088h.3", "Interrupt1EdgeFlag", false),                                        //ie1
            new Bool("088h.2", "Interrupt1SignalType", false),                                      //it1
            new Bool("088h.1", "Interrupt0EdgeFlag", false),                                        //ie0
            new Bool("088h.0", "Interrupt0SignalType", false),                                      //it0
                //InterruptEnable
            new Bool("088h.7", "EnableAllInterrupts", false),                                       //ea
            new Bool("0A8h.5", "EnableTimer2Interrupt", false),                                     //et2
            new Bool("0A8h.4", "EnableSerialPortInterrupt", false),                                 //es
            new Bool("0A8h.3", "EnableTimer1Interrupt", false),                                     //et1
            new Bool("0A8h.2", "EnableExternalInterrupt1", false),                                  //ex1
            new Bool("0A8h.1", "EnableTimerInterrupt0", false),                                     //et0
            new Bool("0A8h.0", "EnableExternalInterrupt0", false),                                  //ex0
                //InterruptPriority
            new Bool("0B8h.5", "Timer2InterruptPriority", false),                                   //pt2
            new Bool("0B8h.4", "SerialPortInterruptPriority", false),                               //ps
            new Bool("0B8h.3", "Timer1InterruptPriority", false),                                   //pt1
            new Bool("0B8h.2", "External1InterruptPriority", false),                                //px1
            new Bool("0B8h.1", "Timer0InterruptPriority", false),                                   //pt0
            new Bool("0B8h.0", "External0InterruptPriority", false),                                //px0
                //Port3
            new Bool("0B0h.7", "DataMemoryRead", false),                                            //rd
            new Bool("0B0h.6", "DataMemoryWrite", false),                                           //wr
            new Bool("0B0h.5", "Timer1ExternalInput", false),                                       //t1
            new Bool("0B0h.4", "Timer0ExternalInput", false),                                       //t0
            new Bool("0B0h.3", "Interrupt1", false),                                                //int1
            new Bool("0B0h.2", "Interrupt0", false),                                                //int0
            new Bool("0B0h.1", "SerialOutputPort", false),                                          //txd
            new Bool("0B0h.0", "SerialInputPort", false),                                           //rxd
                //Timer2Control
            new Bool("0C8h.7", "Timer2Overflow", false),                                            //tf2
            new Bool("0C8h.6", "Timer2ExternalFlag", false),                                        //exf2
            new Bool("0C8h.5", "ReceiveClockEnable", false),                                        //rclk
            new Bool("0C8h.4", "TransmitClockEnable", false),                                       //tclk
            new Bool("0C8h.3", "Timer2ExternalEnable", false),                                      //exen2
            new Bool("0C8h.2", "Timer2Run", false),                                                 //tr2
            new Bool("0C8h.1", "Counter_Timer2Select", false),                                      //c_t2
            new Bool("0C8h.0", "Capture_Reload2Select", false),                                     //cp_rl2
                //SerialControl
            new Bool("098h.7", "SerialPortMode0", false),                                           //sm0
            new Bool("098h.6", "SerialPortMode1", false),                                           //sm1
            new Bool("098h.5", "MultiprocessorCommunicationsEnable", false),                        //sm2
            new Bool("098h.4", "ReceiverEnable", false),                                            //ren
            new Bool("098h.3", "TransmitBit8", false),                                              //tb8
            new Bool("098h.2", "ReceiveBit8", false),                                               //rb8
            new Bool("098h.1", "TransmitFlag", false),                                              //ti
            new Bool("098h.0", "ReceiveFlag", false),                                               //ri
                //Port1
            new Bool("090h.7", "ClockInput_Output", false),                                         //sck
            new Bool("090h.6", "DataInput_Output", false),                                          //miso
            new Bool("090h.5", "DataOutput_Input", false),                                          //mosi
            new Bool("090h.4", "SlavePortSelectInput", false),                                      //ss
            new Bool("090h.1", "Timer_Counter2Capture_ReloadTrigger_DirectionControl", false),      //t2ex
            new Bool("090h.0", "CountInputTimer_Counter2", false),                                  //t2
            //new Bool(false, "0D0h.7.", "c")                                                       //c
        };

        /// <summary>
        /// The path the TCode gets passed to the compiler
        /// </summary>
        /// <value>The path as a string</value>
        public static string InputPath { get; set; }
        /// <summary>
        /// The path the compiled assembler code gets saved to
        /// </summary>
        /// <value>The path as a string</value>
        public static string OutputPath { get; set; }
        /// <summary>
        /// The path the errors get saved to
        /// </summary>
        /// <value>The path as a string</value>
        public static string ErrorPath { get; set; }

        /// <summary>
        /// The name of the external interrupt 0 execution method
        /// </summary>
        public const string ExternalInterrupt0ExecutionName = "ISRe0";
        /// <summary>
        /// The name of the external interrupt 1 execution method
        /// </summary>
        public const string ExternalInterrupt1ExecutionName = "ISRe1";
    }
}
