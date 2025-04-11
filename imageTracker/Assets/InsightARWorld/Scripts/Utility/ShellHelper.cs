

    using System.Diagnostics;
    using System.Text;

public static class ShellHelper
{
    /// <summary>
    /// Run a shell command.
    /// </summary>
    /// <param name="fileName">File name for the executable.</param>
    /// <param name="arguments">Command line arguments, space delimited.</param>
    /// <param name="output">Filled out with the result as printed to stdout.</param>
    /// <param name="error">Filled out with the result as printed to stderr.</param>
    public static void RunCommand(string fileName, string arguments, out string output, out string error)
    {
        using (var process = new System.Diagnostics.Process())
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo(fileName, arguments);
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;

            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();
            process.OutputDataReceived += (sender, ef) => outputBuilder.AppendLine(ef.Data);
            process.ErrorDataReceived += (sender, ef) => errorBuilder.AppendLine(ef.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            process.Close();

            // Trims the output strings to make comparison easier.
            output = outputBuilder.ToString().Trim();
            error = errorBuilder.ToString().Trim();
        }
    }

    public static void RunCommand(string fileName, string arguments)
    {
        using (var process = new Process())
        {
            var startInfo = new ProcessStartInfo(fileName, arguments);
            startInfo.CreateNoWindow = false;
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}
    /// @endcond

