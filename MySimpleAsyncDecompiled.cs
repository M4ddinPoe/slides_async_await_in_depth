using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

  public class MySimpleAsyncDecompiled
  {
    [DebuggerStepThrough]
    [AsyncStateMachine(typeof(DoSomethingAsyncStateMachine))]
    public Task<int> DoSomethingAsync(int parameter)
    {
      var asyncTaskMethodBuilder = AsyncTaskMethodBuilder<int>.Create();

      DoSomethingAsyncStateMachine methodAsyncStateMachine = 
        new DoSomethingAsyncStateMachine()
          {
            __parameter =
              parameter,
            __this = this,
            __state = -1,
            __builder =
              asyncTaskMethodBuilder
          };

      asyncTaskMethodBuilder.Start(ref methodAsyncStateMachine);
      return methodAsyncStateMachine.__builder.Task;
    }

    private async Task<int> MyMethodAsync(int parameter)
    {
      return await Task.FromResult(parameter + 1);
    }

    private int SomeSynchronousMethod(int parameter)
    {
      return parameter + 1;
    }

    public sealed class DoSomethingAsyncStateMachine : IAsyncStateMachine
    {
      public AsyncTaskMethodBuilder<int> __builder;

      public int __state;

      public MySimpleAsyncDecompiled __this;

      public int __parameter;

      private TaskAwaiter<int> __taskAwaiter;

      private int __result;

      void IAsyncStateMachine.MoveNext()
      {
        try
        {
          // Wenn im initialen Status
          if (this.__state != 0)
          {
            // Ausführen des synchronen Codes vor dem ersten await
            this.__result = this.__this.SomeSynchronousMethod(
              this.__parameter);

            // Ausführen des asynchronen Codes
            this.__taskAwaiter = this.__this.MyMethodAsync(
              this.__parameter).GetAwaiter();

            // Wenn der Task schon fertig ist; bleiben wir synchron und geben 
            // das Ergebnis zurück
            // Ansonsten machen wir in dem folgenden Block weiter
            if (!this.__taskAwaiter.IsCompleted)
            {
              this.__state = 0;
              DoSomethingAsyncStateMachine stateMachine = this;

              this.__builder.AwaitUnsafeOnCompleted(
                ref this.__taskAwaiter, ref stateMachine);

              return;
            }
          }

          // Für weitere async Methoden werden hier weitere Stati definiert. 
          // Die Stati werden dann positiv hochgezählt

          // Hier wird das Result aus dem Task geholt, der State auf  
          // completed gesetzt und das Result zurück gegeben
          this.__result = this.__taskAwaiter.GetResult();
          this.__state = -2;
          this.__builder.SetResult(__result);
        }
        catch (Exception exception)
        {
          // Exceptions werden hier gefangen und über den Builder an den 
          // Aufrufer weiter gegeben
          this.__state = -2;
          this.__builder.SetException(exception);
        }
      }

      public void SetStateMachine(IAsyncStateMachine stateMachine)
      {
      }
    }

  }