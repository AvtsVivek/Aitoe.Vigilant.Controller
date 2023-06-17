using CommandLine;
using System;
using System.IO;

namespace Aitoe.Vigilant.CLP
{
    public abstract class ProcessorTemplateBase<TOptions> where TOptions : new()
    {
        protected TextWriter Error;
        protected TextReader Input;
        protected TextWriter Output;
        protected TOptions Options;

        ///<summary>
        ///The Template Method.
        ///
        /// Defines the series of steps, some of which 
        /// may be overriden in the derived class.
        ///</summary>
        ///<param name="args">The command line arguemnts</param>
        ///<param name="input">The input stream that will be iterated over</param>
        ///<param name="output">The output stream</param>
        ///<param name="error">the error streadm</param>
        public void Process(string[] args, TextReader input, TextWriter output, TextWriter error)
        {
            Error = error;
            Input = input;
            Output = output;

            ParserOptions(args);

            var isValidArguments = ValidateArguments();

            if (isValidArguments)
            {
                PreProcess();
                ProcessLines();
                PostProcess();
            }
        }

        private void ParserOptions(string[] args)
        {
            Options = new TOptions();

            var parsingSucceeded = Parser.Default.ParseArgumentsStrict(args, Options, OnFail);


        }

        private void OnFail()
        {
            Output.WriteLine("Parsing Failed.");
            Environment.Exit((int)ExitCodes.ParsingFailure);
        }

        protected virtual void PostProcess()
        {

        }

        /// <summary>
        /// Override to perform one-time pre-processing 
        /// that executes before the main processing loop.
        /// </summary>
        protected virtual void PreProcess()
        {

        }

        protected virtual void ProcessLines()
        {
            //In my case. the command line should process in one shot. No waiting for user input.
            //var currentLine = Input.ReadLine();

            //while (currentLine != null)
            //{
            //ProcessLine(currentLine);
            //    currentLine = Input.ReadLine();
            //}
        }

        protected virtual void ProcessLine(string currentLine)
        {

        }

        /// <summary>
        /// Overrides to perform additional arguemnt validation
        /// and write validation error output to user.
        /// Defaults to true.
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateArguments()
        {
            return true;
        }
    }
}
