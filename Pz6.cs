using System;
using System.Threading.Tasks;

namespace StateMachineDemo
{
    public enum State
    {
        Stopped,
        Playing,
        Paused
    }

    public class MusicPlayer
    {
        private State _currentState = State.Stopped;

        public async Task PlayAsync()
        {
            Console.WriteLine(" -> User pressed Play...");
            await Task.Delay(1000);

            if (_currentState == State.Playing)
            {
                Console.WriteLine("Error: Music is already playing.");
            }
            else
            {
                _currentState = State.Playing;
                Console.WriteLine($"Success: Playing. State: {_currentState}");
            }
        }

        public async Task PauseAsync()
        {
            Console.WriteLine(" -> User pressed Pause...");
            await Task.Delay(500);

            if (_currentState == State.Playing)
            {
                _currentState = State.Paused;
                Console.WriteLine($"Success: Paused. State: {_currentState}");
            }
            else if (_currentState == State.Paused)
            {
                Console.WriteLine("Error: Already paused.");
            }
            else
            {
                Console.WriteLine("Error: Cannot pause while stopped.");
            }
        }

        public async Task StopAsync()
        {
            Console.WriteLine(" -> User pressed Stop...");
            await Task.Delay(500);

            if (_currentState == State.Stopped)
            {
                Console.WriteLine("Info: Already stopped.");
            }
            else
            {
                _currentState = State.Stopped;
                Console.WriteLine($"Success: Stopped. State: {_currentState}");
            }
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var player = new MusicPlayer();
            Console.WriteLine("--- Commands: play, pause, stop, exit ---");

            while (true)
            {
                Console.Write("\nEnter command: ");
                string command = Console.ReadLine()?.Trim().ToLower();

                switch (command)
                {
                    case "play":
                        await player.PlayAsync();
                        break;
                    case "pause":
                        await player.PauseAsync();
                        break;
                    case "stop":
                        await player.StopAsync();
                        break;
                    case "exit":
                        return;
                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }
        }
    }
}
