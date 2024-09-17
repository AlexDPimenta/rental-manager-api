# API de Aluguel de Motos

Esta API permite o gerenciamento e locação de motos, assim como o cadastro de entregadores. A aplicação utiliza o padrão Vertical Slice Architecture, que organiza o código por funcionalidades ao invés de camadas, facilitando a manutenção e escalabilidade do projeto. Além disso, a API faz uso do Kafka para processamento de eventos, Azure Storage para armazenamento de dados, e PostgreSQL como banco de dados.

## Funcionalidade

- Cadastro de Motos: Permite cadastrar novas motos na base de dados.
- Cadastro de Entregadores: Permite o cadastro de entregadores para vinculação nas locações.
- Locação de Motos: Gera uma locação associada a um entregador e a uma moto.
- Cálculo de Valor Devido na Entrega: Calcula o valor total a ser pago na entrega da moto, incluindo eventuais taxas.

## Padrão Vertical Slice

A API utiliza o padrão Vertical Slice, que consiste em organizar o código em camadas verticais, onde cada camada é responsável por uma funcionalidade específica. Isso permite uma melhor separação e facilita a manutenção e evolução do sistema.

## Tecnologias Utilizadas

A API utiliza as seguintes tecnologias:

- Minimal API com Carter: Facilita a criação de rotas e handlers de forma simplificada e modular.
- Kafka: Utilizado para processamento assíncrono de eventos, como notificações e logs de locações.
- MassTransit: Usado para orquestrar a mensageria e eventos entre os serviços via Kafka.
- Azure Storage: Utilizado para armazenar arquivos como cnh.
- PostgreSQL: Banco de dados relacional para armazenar as informações das motos, entregadores, e locações.
- Docker Compose: Facilita a configuração e execução de todos os serviços necessários (API, banco de dados, Kafka) em contêineres.

## Executando a API com Docker Compose

Para executar a API utilizando o Docker Compose, siga os passos abaixo:

1. Certifique-se de ter o Docker e o Docker Compose instalados em sua máquina.
2. Abra um terminal na pasta raiz do projeto.
3. Execute o comando `docker-compose up -d` para iniciar os containers da API.
4. Aguarde até que todos os containers estejam em execução.
5. Abra o kafka através do server localhost:9090 com algum offset de sua preferência e crie o tópico: topic.dev.rental-manager.motor-cycle-created.
6. Restartar a imagem rentalmanager-rentalmanager.webapi-1 através do comando  `docker restart rentalmanager-rentalmanager.webapi-1` , garantindo que todos serviços sejam executados normalmente.
5. A API estará disponível no endereço `http://localhost:5000`.
