### Projeyi Çalıştırma

Projeye eklenen Docker desteği sayesinde .Net Core runtime ihtiyacı olmadan Docker üzerinde çalışabilmektedir.
Öncelikle aşağıdaki komut ile projeyi geliştirme ortamınıza kopyalayabilirsiniz.

`git clone https://github.com/yusufduyar/loan-decisioning.git`

Sonrasında proje ana klasörü içindeki Dockerfile yardımı ile aşağıdaki komutla bir image oluşturabilirsiniz.

`docker build -t loan-api .`

Image oluşturma işlemi bittikten sonra oluşturulan image ile container ayağa kaldırıp uygulamayı test edebilirsiniz.

`docker run -d -p 5000:5000 loan-api`

Uygulama container içinde çalışmaya başladıktan sonra `5000` portu üzerinden istekte bulunabilirsiniz.

Servise ait swagger arayüzüne ise uygulama çalıştıktan sonra aşağıdaki link üzerinden erişip testlerinizi yapabilirsiniz.

`http://localhost:5000/swagger`