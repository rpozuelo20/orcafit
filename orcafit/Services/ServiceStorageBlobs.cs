using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using orcafit.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Services
{
    public class ServiceStorageBlobs
    {
        private BlobServiceClient client;
        public ServiceStorageBlobs(BlobServiceClient client)
        {
            this.client = client;
        }

        //  METODO PARA MOSTRAR TODOS LOS BLOBS DE UN CONTAINER.
        public async Task<List<BlobClass>> GetBlobsAsync(string containerName)
        {
            //  CUALQUIER ACCION SOBRE UN BLOB DIRECTAMENTE NECESITA UN CLIENTE DE BLOBCONTAINER.
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerName);
            List<BlobClass> blobs = new List<BlobClass>();
            await foreach (BlobItem item in containerClient.GetBlobsAsync())
            {
                //  PARA PODER ACCEDER A LA INFORMACION COMPLETA DE UN BLOB NECESITAMOS UN BlobClient A PARTIR DEL NOMBRE DEL BLOB.
                BlobClient blobClient = containerClient.GetBlobClient(item.Name);
                BlobClass blobClass = new BlobClass
                {
                    Nombre = item.Name,
                    Url = blobClient.Uri.AbsoluteUri
                };
                blobs.Add(blobClass);
            }
            return blobs;
        }
        //  METODO PARA RECUPERAR UN SOLO BLOB.
        public async Task<BlobClass> GetBlobAsync(string containerName, string blobName)
        {
            List<BlobClass> blobs = await this.GetBlobsAsync(containerName);
            BlobClass blob = blobs.SingleOrDefault(x => x.Nombre == blobName);
            return blob;
        }
        //  METODO PARA ELIMINAR UN BLOB.
        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerName);
            await containerClient.DeleteBlobAsync(blobName);
        }
        //  METODO PARA SUBIR BLOB A AZURE.
        public async Task UploadBlobAsync(string containerName, string blobName, Stream stream)
        {
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerName);
            await containerClient.UploadBlobAsync(blobName, stream);
        }
    }
}
