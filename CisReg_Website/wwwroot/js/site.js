// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

      //Selecionar os elementos
      document.addEventListener('DOMContentLoaded', () => {
        const cnesInput = document.getElementById('cnes');
        const cnesError = document.getElementById('cnes-error');

        // Adicionar evento de foco (quando o campo é clicado)
    cnesInput.addEventListener('focus', () => {
        cnesInput.classList.add('input-focused'); // Adiciona a classe para destaque
    });

    // Remover o destaque quando o campo perde o foco
    cnesInput.addEventListener('blur', () => {
        cnesInput.classList.remove('input-focused'); // Remove a classe para destaque
    });
    
        cnesInput.addEventListener('input', () => {
            const cnesValue = cnesInput.value;
    
            // Verifica se o valor contém exatamente 7 dígitos numéricos
            if (/^\d{7}$/.test(cnesValue)) {
                cnesError.style.display = 'none'; // Esconde a mensagem de erro
                cnesInput.style.borderColor = ''; // Remove o destaque do campo
            } else {
                cnesError.style.display = 'block'; // Exibe a mensagem de erro
                cnesInput.style.borderColor = 'red'; // Adiciona destaque ao campo
            }
        });
    });

    document.addEventListener('DOMContentLoaded', () => {
        const cnpjInput = document.getElementById('cnpj');
        const cnpjError = document.getElementById('cnpj-error');
    
        cnpjInput.addEventListener('input', () => {
            const cnpjValue = cnpjInput.value.replace(/\D/g, ''); // Remove caracteres não numéricos
    
            // Valida o CNPJ
            if (isValidCNPJ(cnpjValue)) {
                cnpjError.style.display = 'none'; // Esconde a mensagem de erro
                cnpjInput.style.borderColor = ''; // Remove o destaque do campo
            } else {
                cnpjError.style.display = 'block'; // Exibe a mensagem de erro
                cnpjInput.style.borderColor = 'red'; // Adiciona destaque ao campo
            }
        });
    
        function isValidCNPJ(cnpj) {
            // Verifica se o CNPJ tem 14 dígitos
            if (cnpj.length !== 14) return false;
    
            // Elimina CNPJs conhecidos como inválidos
            if (/^(\d)\1+$/.test(cnpj)) return false;
    
            // Calcula os dígitos verificadores
            let t = cnpj.length - 2,
                d = cnpj.substring(t),
                d1 = parseInt(d.charAt(0)),
                d2 = parseInt(d.charAt(1)),
                calc = x => {
                    let n = 0, y = 2;
                    for (let i = x; i >= 0; i--) {
                        n += parseInt(cnpj.charAt(i)) * y;
                        y = (y == 9) ? 2 : y + 1;
                    }
                    return n % 11 < 2 ? 0 : 11 - (n % 11);
                };
            return calc(t - 1) === d1 && calc(t) === d2;
        }
    });

    document.addEventListener('DOMContentLoaded', () => {
        const enderecoInput = document.getElementById('endereco');
        const enderecoError = document.getElementById('endereco-error');
    
        enderecoInput.addEventListener('input', () => {
            const enderecoValue = enderecoInput.value.trim();
    
            // Valida o endereço: pelo menos 10 caracteres
            if (enderecoValue.length >= 10) {
                enderecoError.style.display = 'none'; // Esconde a mensagem de erro
                enderecoInput.style.borderColor = ''; // Remove o destaque do campo
            } else {
                enderecoError.style.display = 'block'; // Exibe a mensagem de erro
                enderecoInput.style.borderColor = 'red'; // Adiciona destaque ao campo
            }
        });
    });
    
  document.addEventListener('DOMContentLoaded', () => {
    const numeroConvenioInput = document.getElementById('numeroConvenio');
    const numeroConvenioError = document.getElementById('numeroConvenio-error');

    numeroConvenioInput.addEventListener('input', () => {
        const numeroConvenioValue = numeroConvenioInput.value.trim();

        // Valida se o número do convênio contém apenas dígitos e está entre 6 e 20 caracteres
        const isValidNumeroConvenio = /^\d{6,20}$/.test(numeroConvenioValue);

        if (isValidNumeroConvenio) {
            numeroConvenioError.style.display = 'none'; // Esconde a mensagem de erro
            numeroConvenioInput.style.borderColor = ''; // Remove o destaque do campo
        } else {
            numeroConvenioError.style.display = 'block'; // Exibe a mensagem de erro
            numeroConvenioInput.style.borderColor = 'red'; // Adiciona destaque ao campo
        }
    });
});

document.addEventListener('DOMContentLoaded', () => {
    const telefoneInput = document.getElementById('telefoneResponsavel');
    const telefoneError = document.getElementById('telefoneResponsavel-error');

    telefoneInput.addEventListener('input', () => {
        let telefoneValue = telefoneInput.value.replace(/\D/g, ''); // Remove todos os caracteres não numéricos

        // Aplica a formatação (XX) XXXXX-XXXX ou (XX) XXXX-XXXX
        if (telefoneValue.length > 10) {
            telefoneValue = telefoneValue.replace(/^(\d{2})(\d{5})(\d{4})$/, '($1) $2-$3');
        } else if (telefoneValue.length > 6) {
            telefoneValue = telefoneValue.replace(/^(\d{2})(\d{4})(\d{0,4})$/, '($1) $2-$3');
        } else if (telefoneValue.length > 2) {
            telefoneValue = telefoneValue.replace(/^(\d{2})(\d{0,5})$/, '($1) $2');
        } else if (telefoneValue.length > 0) {
            telefoneValue = telefoneValue.replace(/^(\d{0,2})$/, '($1');
        }

        telefoneInput.value = telefoneValue;

        // Valida o telefone (10 ou 11 dígitos)
        const isValidTelefone = telefoneValue.replace(/\D/g, '').length >= 10 && telefoneValue.replace(/\D/g, '').length <= 11;

        if (isValidTelefone) {
            telefoneError.style.display = 'none'; // Esconde a mensagem de erro
            telefoneInput.style.borderColor = ''; // Remove o destaque do campo
        } else {
            telefoneError.style.display = 'block'; // Exibe a mensagem de erro
            telefoneInput.style.borderColor = 'red'; // Adiciona destaque ao campo
        }
    });
});

document.addEventListener('DOMContentLoaded', () => {
    const emailInput = document.getElementById('emailResponsavel');
    const emailError = document.getElementById('emailResponsavel-error');

    emailInput.addEventListener('input', () => {
        // Expressão regular para validar e-mail
        const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

        // Valida o e-mail
        const isValidEmail = emailRegex.test(emailInput.value);

        if (isValidEmail) {
            emailError.style.display = 'none'; // Esconde a mensagem de erro
            emailInput.style.borderColor = ''; // Remove o destaque do campo
        } else {
            emailError.style.display = 'block'; // Exibe a mensagem de erro
            emailInput.style.borderColor = 'red'; // Adiciona destaque ao campo
        }
    });
});

document.addEventListener('DOMContentLoaded', () => {
    const responsavelInput = document.getElementById('responsavelPrefeitura');
    const responsavelError = document.getElementById('responsavelPrefeitura-error');

    responsavelInput.addEventListener('input', () => {
        // Verifica se o campo foi preenchido
        const isValid = responsavelInput.value.trim() !== '';

        if (isValid) {
            responsavelError.style.display = 'none'; // Esconde a mensagem de erro
            responsavelInput.style.borderColor = ''; // Remove o destaque do campo
        } else {
            responsavelError.style.display = 'block'; // Exibe a mensagem de erro
            responsavelInput.style.borderColor = 'red'; // Adiciona destaque ao campo
        }
    });
});
