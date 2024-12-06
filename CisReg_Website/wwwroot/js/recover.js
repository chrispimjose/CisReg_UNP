
// Views - Recover - Index.cshtml

// Função listener para verificar e adicionar o elemento de id 'errorToast' e fazer ele desaparecer.
document.addEventListener('DOMContentLoaded', function () {
    const errorAlert = document.getElementById('errorAlert');
    errorAlert.style.opacity = 1;

    let alertOpacity = parseFloat(errorAlert.style.opacity);

    function fadeAlert() {
        alertOpacity -= 0.03;
        errorAlert.style.opacity = alertOpacity;

        if (alertOpacity <= 0) {
            errorAlert.style.display = 'none';
        } else {
            requestAnimationFrame(fadeAlert);
        }
    }

    setTimeout(fadeAlert, 2000);

});


// Função que torna o password digitado visível por meio de uma troca de type (password -> text || text -> password), assim como o ícone.
function togglePasswordVisibility() {
    var passwordField = document.getElementById('password');
    var toggleIcon = document.getElementById('toggle-icon');
    if (passwordField.type === 'password') {
        passwordField.type = 'text';
        toggleIcon.classList.remove('fa-eye');
        toggleIcon.classList.add('fa-eye-slash');
    } else {
        passwordField.type = 'password';
        toggleIcon.classList.remove('fa-eye-slash');
        toggleIcon.classList.add('fa-eye');
    }
}

// Função para estilizar o campo de E-mail, impedindo a utilização do espaço e deixando-o apenas em letras minúsculas.
function formatEmailInputAtRecover(email) {
    email = email.trim();

    email = email.toLowerCase();

    email = email.replace(/\s+/g, '');

    return email
}

// Função a ser chamada em Personal Info que envia o input atual e faz a transformação.
function applyEmailFormatAtRecover(input) {
    input.value = formatEmailInputAtRecover(input.value);
}

function formatCodeInputAtRecover(code) {
    code = code.trim();

    code = code.replace(/\D/g, '');

    code = code.replace(/\s+/g, '');

    return code
}

function applyCodeFormatAtRecover(input) {
    input.value = formatCodeInputAtRecover(input.value);
}

function validateEmail(input, errorMessageText) {

    const email = input.value.trim();
    const emailPattern = /^[^\s@]+@[^\s@]+\.[a-zA-Z]{2,}$/;

    const span = document.querySelector(`#${input.id}-error`);

    input.classList.remove('input-error', 'input-accent');

    const isEmpty = email == '';

    const validTLDs = ['com', 'org', 'net', 'edu', 'gov', 'br', 'co', 'uk', 'de', 'fr', 'us', 'ca', 'au', 'jp'];

    if (isEmpty) {
        input.classList.add('input-error');
        span.classList.add('hidden');
        span.innerHTML = "";
        return;
    }

    if (!emailPattern.test(email)) {
        input.classList.add('input-error');
        span.classList.remove('hidden');
        span.innerHTML = errorMessageText;
        return;
    }

    // Verifica se o domínio possui um TLD válido
    const domain = email.split('@')[1];
    const tld = domain.split('.').pop();

    if (!validTLDs.includes(tld.toLowerCase())) {
        input.classList.add('input-error');
        span.classList.remove('hidden');
        span.innerHTML = "Por favor, insira um domínio válido.";
        return;
    }

    input.classList.add('input-accent');
    span.classList.add('hidden');
    span.innerHTML = "";


}

// Função que valida o formato do valor de inserção atribuído a este campo.
function validateFieldInputAtLogin(input) {
    input.classList.remove('input-error');
    input.classList.remove('input-accent');

    if (!input.checkValidity()) {
        input.classList.add('input-error');
    } else {
        input.classList.add('input-accent');
    }
}

// Função que valida o formato do valor de inserção atribuído a este campo, exibindo/removendo textos e efeitos de erro.
function validateFieldInput(input, errorMessageText) {

    const span = document.querySelector(`#${input.id}-error`);

    input.classList.remove('input-error', 'input-accent');

    const isValid = input.checkValidity();
    const isEmpty = input.value.trim() === '';

    // Verifica a validade do input de acordo com o pattern.
    if (!isValid) {
        input.classList.add('input-error');
        span.classList.remove('hidden');
        span.innerHTML = errorMessageText;
    }
    else {
        input.classList.add('input-accent');
        span.classList.add('hidden');
        span.innerHTML = "";
    }
    if (isEmpty) {
        input.classList.add('input-error');
        span.classList.add('hidden');
        span.innerHTML = "";
    }
}


document.body.classList.add('no-scroll');











