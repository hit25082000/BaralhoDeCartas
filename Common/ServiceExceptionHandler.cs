using BaralhoDeCartas.Exceptions;

namespace BaralhoDeCartas.Common
{
    public static class ServiceExceptionHandler
    {
        public static async Task<T> HandleServiceExceptionAsync<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (BaralhoNotFoundException)
            {
                throw;
            }
            catch (ExternalServiceUnavailableException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro interno no serviço", ex);
            }
        }

        public static T HandleServiceException<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            catch (BaralhoNotFoundException)
            {
                throw;
            }
            catch (ExternalServiceUnavailableException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro interno no serviço", ex);
            }
        }
    }
} 