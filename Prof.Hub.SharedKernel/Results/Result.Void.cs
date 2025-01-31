namespace Prof.Hub.SharedKernel.Results
{
    public class Result : Result<Result>
    {
        public Result() : base() { }

        protected internal Result(ResultStatus status) : base(status) { }

        /// <summary>
        /// Representa uma operação bem-sucedida sem tipo de retorno.
        /// </summary>
        /// <returns>Um Result</returns>
        public static Result Success() => new();

        /// <summary>
        /// Representa uma operação bem-sucedida sem tipo de retorno.
        /// </summary>
        /// <param name="successMessage">Define a propriedade SuccessMessage</param>
        /// <returns>Um Result</returns>
        public static Result SuccessWithMessage(string successMessage) => new() { SuccessMessage = successMessage };

        /// <summary>
        /// Representa uma operação bem-sucedida e aceita um valor como resultado da operação.
        /// </summary>
        /// <param name="value">Define a propriedade Value</param>
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> Success<T>(T value) => new(value);

        /// <summary>
        /// Representa uma operação bem-sucedida e aceita um valor como resultado da operação.
        /// Define a propriedade SuccessMessage para o valor fornecido.
        /// </summary>
        /// <param name="value">Define a propriedade Value</param>
        /// <param name="successMessage">Define a propriedade SuccessMessage</param>
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> Success<T>(T value, string successMessage) => new(value, successMessage);

        /// <summary>
        /// Representa uma operação bem-sucedida que resultou na criação de um novo recurso.
        /// Aceita um valor como resultado da operação.
        /// </summary>		
        /// <param name="value">Define a propriedade Value</param>
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> Created<T>(T value)
        {
            return Result<T>.Created(value);
        }

        /// <summary>
        /// Representa uma operação bem-sucedida que resultou na criação de um novo recurso.
        /// Aceita um valor como resultado da operação.
        /// Aceita uma localização para o novo recurso.
        /// </summary>		
        /// <param name="value">Define a propriedade Value</param>
        /// <param name="location">A localização do recurso recém-criado</param>
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> Created<T>(T value, string location)
        {
            return Result<T>.Created(value, location);
        }

        /// <summary>
        /// Representa um erro que ocorreu durante a execução do serviço.
        /// Mensagens de erro podem ser fornecidas e serão expostas pela propriedade Errors.
        /// </summary>
        /// <param name="error">Uma instância opcional de ErrorList com a lista de mensagens de erro em string e CorrelationId.</param>
        /// <returns>Um Result</returns>
        public new static Result Error(ErrorList error = null) => new(ResultStatus.Error)
        {
            CorrelationId = error?.CorrelationId ?? string.Empty,
            Errors = error?.ErrorMessages ?? []
        };

        /// <summary>
        /// Representa um erro que ocorreu durante a execução do serviço.
        /// Uma única mensagem de erro pode ser fornecida e será exposta pela propriedade Errors.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns>Um Result</returns>
        public new static Result Error(string errorMessage) => new(ResultStatus.Error) { Errors = new[] { errorMessage } };

        /// <summary>
        /// Representa o erro de validação que impede a conclusão do serviço subjacente.
        /// </summary>
        /// <param name="validationError">O erro de validação encontrado</param>
        /// <returns>Um Result</returns>
        public new static Result Invalid(ValidationError validationError)
            => new(ResultStatus.Invalid) { ValidationErrors = [validationError] };

        /// <summary>
        /// Representa erros de validação que impedem a conclusão do serviço subjacente.
        /// </summary>
        /// <param name="validationErrors">Uma lista de erros de validação encontrados</param>
        /// <returns>Um Result</returns>
        public new static Result Invalid(params ValidationError[] validationErrors)
            => new(ResultStatus.Invalid) { ValidationErrors = new List<ValidationError>(validationErrors) };

        /// <summary>
        /// Representa erros de validação que impedem a conclusão do serviço subjacente.
        /// </summary>
        /// <param name="validationErrors">Uma lista de erros de validação encontrados</param>
        /// <returns>Um Result</returns>
        public new static Result Invalid(IEnumerable<ValidationError> validationErrors)
            => new(ResultStatus.Invalid) { ValidationErrors = validationErrors };

        /// <summary>
        /// Representa a situação em que um serviço não conseguiu encontrar um recurso solicitado.
        /// </summary>
        /// <returns>Um Result</returns>
        public new static Result NotFound() => new Result(ResultStatus.NotFound);

        /// <summary>
        /// Representa a situação em que um serviço não conseguiu encontrar um recurso solicitado.
        /// Mensagens de erro podem ser fornecidas e serão expostas pela propriedade Errors.
        /// </summary>
        /// <param name="errorMessages">Uma lista de mensagens de erro em string.</param>
        /// <returns>Um Result</returns>
        public new static Result NotFound(params string[] errorMessages) => new(ResultStatus.NotFound) { Errors = errorMessages };

        /// <summary>
        /// Os parâmetros da chamada estavam corretos, mas o usuário não tem permissão para realizar alguma ação.
        /// </summary>
        /// <returns>Um Result</returns>
        public new static Result Forbidden() => new(ResultStatus.Forbidden);

        /// <summary>
        /// Os parâmetros da chamada estavam corretos, mas o usuário não tem permissão para realizar alguma ação.
        /// </summary>
        /// <param name="errorMessages">Uma lista de mensagens de erro em string.</param> 
        /// <returns>Um Result</returns>
        public new static Result Forbidden(params string[] errorMessages) => new(ResultStatus.Forbidden) { Errors = errorMessages };

        /// <summary>
        /// Isso é semelhante a Proibido, mas deve ser usado quando o usuário não se autenticou ou tentou se autenticar, mas falhou.
        /// </summary>
        /// <returns>Um Result</returns>
        public new static Result Unauthorized() => new(ResultStatus.Unauthorized);

        /// <summary>
        /// Isso é semelhante a Proibido, mas deve ser usado quando o usuário não se autenticou ou tentou se autenticar, mas falhou.
        /// </summary>
        /// <param name="errorMessages">Uma lista de mensagens de erro em string.</param>  
        /// <returns>Um Result</returns>
        public new static Result Unauthorized(params string[] errorMessages) => new(ResultStatus.Unauthorized) { Errors = errorMessages };

        /// <summary>
        /// Representa uma situação em que um serviço está em conflito devido ao estado atual de um recurso,
        /// como um conflito de edição entre várias atualizações concorrentes.
        /// </summary>
        /// <returns>Um Result</returns>
        public new static Result Conflict() => new(ResultStatus.Conflict);

        /// <summary>
        /// Representa uma situação em que um serviço está em conflito devido ao estado atual de um recurso,
        /// como um conflito de edição entre várias atualizações concorrentes.
        /// Mensagens de erro podem ser fornecidas e serão expostas pela propriedade Errors.
        /// </summary>
        /// <param name="errorMessages">Uma lista de mensagens de erro em string.</param>
        /// <returns>Um Result</returns>
        public new static Result Conflict(params string[] errorMessages) => new(ResultStatus.Conflict) { Errors = errorMessages };

        /// <summary>
        /// Representa uma situação em que um serviço está indisponível, como quando o armazenamento de dados subjacente está indisponível.
        /// Os erros podem ser transitórios, então o chamador pode desejar tentar a operação novamente.
        /// </summary>
        /// <param name="errorMessages">Uma lista de mensagens de erro em string.</param>
        /// <returns></returns>
        public new static Result Unavailable(params string[] errorMessages) => new(ResultStatus.Unavailable) { Errors = errorMessages };

        /// <summary>
        /// Representa um erro crítico que ocorreu durante a execução do serviço.
        /// Tudo fornecido pelo usuário era válido, mas o serviço não conseguiu concluir devido a uma exceção.
        /// </summary>
        /// <param name="errorMessages">Uma lista de mensagens de erro em string.</param>
        /// <returns>Um Result</returns>
        public new static Result CriticalError(params string[] errorMessages) => new(ResultStatus.CriticalError) { Errors = errorMessages };

        /// <summary>
        /// Representa uma situação em que o servidor atendeu com sucesso a solicitação, mas não há conteúdo para enviar de volta no corpo da resposta.
        /// </summary>
        /// <returns>Um objeto Result</returns>
        public new static Result NoContent() => new(ResultStatus.NoContent);
    }
}
