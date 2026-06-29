using System;
using System.Runtime.InteropServices;

namespace BraveAdventurersGuild.Access
{
    /// <summary>
    /// Ponte profissional para a Tolk DLL.
    /// Permite que o jogo se comunique com NVDA, JAWS e outros leitores de tela.
    /// </summary>
    public static class TolkHandler
    {
        [DllImport("Tolk.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void Tolk_Load();

        [DllImport("Tolk.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void Tolk_Unload();

        [DllImport("Tolk.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern bool Tolk_IsLoaded();

        [DllImport("Tolk.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern bool Tolk_Speak(string text, bool interrupt);

        [DllImport("Tolk.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern bool Tolk_Silence();
    }

    public class ScreenReader
    {
        public ScreenReader()
        {
            try 
            {
                TolkHandler.Tolk_Load();
            }
            catch (Exception e)
            {
                // Em ambiente de desenvolvimento sem a DLL, logamos o erro
                Console.WriteLine($"Erro ao carregar Tolk: {e.Message}");
            }
        }

        public void Say(string text, bool interrupt = true)
        {
            if (TolkHandler.Tolk_IsLoaded())
            {
                TolkHandler.Tolk_Speak(text, interrupt);
            }
            else
            {
                // Fallback para o console se a Tolk não estiver ativa
                Console.WriteLine($"[SCREEN READER]: {text}");
            }
        }

        public void Shutdown()
        {
            if (TolkHandler.Tolk_IsLoaded())
            {
                TolkHandler.Tolk_Unload();
            }
        }
    }
}
