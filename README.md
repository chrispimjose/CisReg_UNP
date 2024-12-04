# Regulagem CIS - Centro Integrado de Saúde

## Objetivo

O projeto **Regulagem CIS** visa criar um sistema eficiente para o gerenciamento e regulação das vagas no ambulatório da Universidade Potiguar (UNP). O sistema tem como principais objetivos:

- **Oferta de Vagas:** Permitir ao ambulatório disponibilizar e gerenciar vagas para os municípios encaminharem pacientes. O sistema deve manter um registro atualizado das vagas disponíveis para consultas, exames e outros procedimentos especializados.
  
- **Regulação dos Encaminhamentos:** Receber e processar os encaminhamentos dos municípios, que geralmente vêm de unidades de atenção básica, para garantir que pacientes necessitados de atendimento especializado possam ser atendidos de forma eficiente e organizada.

## Como rodar o projeto?

- Para o projeto utilizamos: ASP.NET Core, C#, MongoDb, TailwindCss, Daisy Ui, Docker (opcional) e mais algumas ferramentas menores.

1. Clone o projeto numa pasta acessível
2. Instale o pacote [.NET mais recente](https://dotnet.microsoft.com/en-us/download) (Utilizado no projeto 8.0.110) e a versão [mais recente do nodejs](https://nodejs.org/en/download/prebuilt-installer) (Utilizado no projeto 20.13.1).
3. Entre no projeto e instale as dependências do .NET com `dotnet restore` e as do node com `npm install`
4. Abra um terminal e rode `npm run build:css` para compilar o TailwindCss
5. Abra um novo terminal, sem fechar o do Tailwind, e rode o .NET com `dotnet watch run`

- Feito, você vai estar rodando o projeto com o servidor Mongodb de produção. Se você possuir um pouco a mais de domínio nas tecnologias, pode tentar conectar com o MongoDb local baixando na sua própria máquina ou rodando em Docker.

- O docker-compose.yml já está configurado com um serviço de mongo e um serviço para acessar o banco por GUI.
