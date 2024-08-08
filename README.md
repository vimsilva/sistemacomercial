### README.md

# Sistema Comercial

## Contexto

Esta aplicação de controle de fluxo de caixa é projetada para gerenciar lançamentos financeiros, incluindo débitos e créditos, e fornecer um saldo consolidado diário. O sistema é composto por dois serviços principais: `Lancamentos` para registrar lançamentos financeiros e `SaldoConsolidado` para calcular e manter o saldo diário consolidado com base nos lançamentos registrados.

## Arquitetura e Tecnologias

- **.NET 8**
- **Entity Framework Core 8**
- **PostgreSQL**
- **RabbitMQ**
- **Docker**
- **Docker Compose**

## Estrutura da Aplicação

- **Lancamentos**
- **SaldoConsolidado**

### Docker Compose

O arquivo `docker-compose.yml` é utilizado para definir e configurar todos os serviços necessários, incluindo os contêineres de `lancamentos`, `saldoconsolidado`, `PostgreSQL` e `RabbitMQ`.

## Comandos para Subir os Containers

1. **Clone o repositório e navegue até o diretório do projeto:**

   ```sh
   git clone https://github.com/vimsilva/sistemacomercial.git
   cd sistemacomercial
   ```

2. **Construa e suba os contêineres utilizando Docker Compose:**

   ```sh
   docker-compose up --build
   ```

3. **Atualize o banco de dados executando o a linha de comando no container lançamento e saldo-consolidado**

   ```sh
   dotnet ef database update
   ```

## Próximos Passos

Para melhorar a qualidade e a robustez do sistema, considere os seguintes passos:

1. **Implementar Testes Automatizados**
2. **Melhorar a Segurança**
3. **Monitoramento e Logging**

