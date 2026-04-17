# Store Order System

Este projeto é uma solução para processamento assíncrono de pedidos.

---

## Como Executar (Quick Start)

A solução está totalmente conteinerizada, dessa forma basta ter o docker no host e o codigo da aplicação.

### Pré-requisitos
* **Docker** e **Docker Compose** instalados.

### Passo a Passo
1. Clone o repositório.
2. Verifique que possua o docker em sua maquina e que as portas 3000, 8181, 5445, 5672, 15672, 8182 estão livres pois elas serão utilizadas pelos serviços
3. Na raiz do projeto, execute:
   ```bash
   docker compose -f docker-compose.yml -f docker-compose.override.yml up -d
   ```
3. **Aguarde a estabilização**: O sistema utiliza de **Healthchecks**. O Frontend aguarda o Keycloak estar totalmente `healthy` para iniciar sendo assim a inicialização completa dos serviços pode levar algum tempo.
4. **Acesse as interfaces**:
   * **Frontend**: [http://localhost:3000](http://localhost:3000) (usuario_teste/123456)
   * **API Swagger**: [http://localhost:8181/swagger](http://localhost:8181/swagger)
   * **RabbitMQ**: [http://localhost:15672](http://localhost:15672) (guest/guest)
   * **Keycloak**: [http://localhost:8182](http://localhost:8182) (admin/admin)
5. **Usuários de acesso**: O sistema ja vem com um usuário pre-configurado no arquivo de configuração do keycloak presente em keycloak-config/realm-export.json para utilizar ele use as credênciais User:"usuario_teste" Password:"123456"
6. **Cadastrar pedido**: Após acessar o front-end e estar autenticado no keycloak você ira automaticamente para a tela de cadastro de pedidos, nela você devera forncer o numero do pedido o nome do cliente no pedido e o seu valor.
7. **Listagem de pedidos**: Após cadastrar 1 ou mais pedidos você pode ir para a tela de Listagem de Pedidos através do sidebar lateral, uma vez nela, será possivel ver a listagem dos pedidos e buscar pelos pedidos.
8. **Detalhes do pedido**: Na tela de listagem de pedidos você vera que todos os pedidos possuem um botão que direciona o usuário para outra tela em que podemos visualizar o status do pedido bem como as operações realizadas no pedido até aquele momento.

---

## Tecnologias Utilizadas

### Backend (.NET 8)
* **Rebus**: Abstração de Service Bus sobre RabbitMQ para garantir retentativas (retries) e tratamento de erros.
* **EF Core + PostgreSQL**: Persistência relacional com migrações automáticas.
* **Mapster**: Mapeamento de objetos entre as camadas da aplicação.
* **FluentValidation**: Validação de contratos e regras de negócio na api.
* **Testes (xunit)**: Testes automatizados utilizando um contexto com sqLite que simula uma execução real e percorre todos os fluxos da api e worker.

### Frontend (React)
* **TypeScript**: Tipagem estática possibilitando um desenvolvimento mais seguro.
* **Redux**: Gerenciamento de estado global (Autenticação e UI).
* **React Bootstrap**: Componentização visual.
* **Axios**: Interceptores para renovação de token e tratamento global de erros.
* **Toast**: Exibição de mensagens entre a api e o front.
* **keycloak-js**: Autenticação através do keycloack.
* **React Router**: Roteamento das paginas.

### Infraestrutura & Segurança
* **Keycloak**: Identity and Access Management (IAM) utilizando o protocolo OAuth2/OIDC.
* **RabbitMQ**: Message Broker para comunicação assíncrona.
* **Docker Compose**: Orquestração completa da stack de serviços.

---

## Estrutura Arquitetural

A solução foi desenhada seguindo os princípios da **Clean Architecture** e **SOLID**:

* **Store.Domain**: Entidades de negócio, Contratos de Eventos. Totalmente agnóstico a frameworks externos.
* **Store.Application**: Casos de uso, Commands, Queries e Validadores, responsavel pelas regras de negocio.
* **Store.Application.Abstractions**: Interfaces compartilhadas entre o Application e a Infrastructure para que o Application não conheça a Infrastructure e não quebre o principio da inversão da dependência.
* **Store.Infraestructure.Data**: Implementações de acesso a dados, mapeamentos a nivel de banco de dados das entidades e migrações.
* **Store.Infraestructure.Messaging**: Implementação das configuras do Rebus e do RabbitMQ.
* **Store.Api**: Interface REST protegida por JWT que atua como **One-Way Client** para a mensageria.
* **Store.Worker**: Serviço autônomo focado exclusivamente no processamento de segundo plano das mensagens enviadas para as filas.
* **Store.Web**: Front-End em React possibilitando a interação do usuário através de uma interface gráfica.

---

## Decisões Técnicas

### 1. Mensageria com One-Way Client
Para garantir a separação de responsabilidades, a API foi configurada exclusivamente como publicadora (One-Way), enquanto o Worker detém a responsabilidade de consumo. Isso evita conflitos de mensagens e garante que a API não sofra carga desnecessária de processamento de fila.

### 2. Resiliência com Rebus
Utilizei o Rebus para implementar **Second Level Retries**, garantindo assim uma idempotência relativa no processamento da fila.

### 3. Segurança OIDC com Keycloak
A aplicação utiliza o Keycloak para prover segurança via JWT, facilitando o desenvolvimento e a manutenção do codigo.

### 4. Mediator para encaminhamento dos casos de uso
A aplicação utiliza o design pattern para propagação das ações necessarias nos casos de uso, dessa forma a comunicação entre os serviços e a aplicação acontece de forma agnostica, desacoplando as entidades e respeitando os principios S (Princípio da responsabilidade única) e D (Princípio da inversão da dependência) do SOLID.

### 5. Bootstrap
A utilização do bootstrap no desenvolvimento possibilitou um desenvolvimento mais agil e com menos codigo.

## Fluxo do Pedido
1.  **Interface**: Usuário envia o pedido.
2.  **API**: Valida, persiste como `Recebido` e publica no RabbitMQ.
3.  **Worker**: Altera para `EmProcessamento`, executa a lógica e finaliza como `Processado` ou `Erro`.

## Testes automatizados
A aplicação conta com duas classes de teste que percorrem todos os casos de uso da mesma, essas classes são OrderIntegrationTests responsavel por testar o fluxo de cadastramento de pedidos da api ate o worker de processamento e a classe OrderQueryTests que testa as operações de consulta dos pedidos realizadas pelo front-end.

